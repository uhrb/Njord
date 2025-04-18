using System.Linq.Expressions;
using System.Reflection;

namespace Njord.NanoOrm
{

    public static class NanoHelpers
    {
        public static string Convert(Expression expression, List<string> expressionParameterNames, Dictionary<string, object> evaluatedParameters, string evaluatedPrefix)
        {
            var state = new ConversionState { EvaluatedParameters = evaluatedParameters, ExpressionParameters = expressionParameterNames, EvaluatePrefix = evaluatedPrefix };
            return ConvertInternal(expression, state);
        }

        private static string ConvertInternal(Expression expression, ConversionState state)
        {
            return expression switch
            {
                LambdaExpression lambda => HandleLambda(lambda, state),
                BinaryExpression binary => HandleBinaryExpression(binary, state),
                MemberExpression member => HandleMemberExpression(member, state),
                ConstantExpression constant => AddParameter(constant.Value, state),
                UnaryExpression unary => ConvertInternal(unary.Operand, state),
                MethodCallExpression methodCall => HandleMethodCall(methodCall, state),
                NewExpression newExpression => AddParameter(CreateObjectFromNewExpression(newExpression, state), state),
                ConditionalExpression conditional => HandleConditionalExpression(conditional, state),
                ParameterExpression parameter => HandleParameterExpression(parameter, null, state),
                _ => throw new NotSupportedException($"Expression type {expression.GetType()} is not supported.")
            };
        }

        private static string HandleLambda(LambdaExpression lambda, ConversionState state)
        {
            state.ExpressionParameters.Clear();
            state.ExpressionParameters.AddRange(lambda.Parameters.Select(_ => _.Name!));
            return ConvertInternal(lambda.Body, state);
        }

        private static string HandleParameterExpression(ParameterExpression expression, string? memberName, ConversionState state)
        {
            var str = expression.Name!;
            if (!string.IsNullOrEmpty(memberName))
            {
                str = $"{expression.Name}.{memberName}";
            }

            return str;
        }

        private static bool IsNullConstant(Expression expression)
        {
            return expression is ConstantExpression constantExpr && constantExpr.Value == null;
        }

        private static string HandleBinaryExpression(BinaryExpression binary, ConversionState state)
        {
            var left = ConvertInternal(binary.Left, state);
            var right = ConvertInternal(binary.Right, state);

            if (IsNullConstant(binary.Right))
            {
                return binary.NodeType switch
                {
                    ExpressionType.Equal => $"{left} IS NULL",
                    ExpressionType.NotEqual => $"{left} IS NOT NULL",
                    _ => $"({left} {GetSqlOperator(binary.NodeType)} {right})"
                };
            }
            else if (IsNullConstant(binary.Left))
            {
                return binary.NodeType switch
                {
                    ExpressionType.Equal => $"{right} IS NULL",
                    ExpressionType.NotEqual => $"{right} IS NOT NULL",
                    _ => $"({left} {GetSqlOperator(binary.NodeType)} {right})"
                };
            }

            if (binary.NodeType == ExpressionType.Coalesce)
            {
                return $"COALESCE({left}, {right})";
            }

            return $"({left} {GetSqlOperator(binary.NodeType)} {right})";

        }

        private static string AddParameter(object? value, ConversionState state)
        {
            var counter = state.EvaluatedParameters.Count + 1;
            var paramName = $"@{state.EvaluatePrefix}{counter}";
            state.EvaluatedParameters[paramName] = value ?? DBNull.Value;
            return paramName;
        }


        private static string HandleConditionalExpression(ConditionalExpression conditional, ConversionState state)
        {
            var condition = ConvertInternal(conditional.Test, state);
            var ifTrue = ConvertInternal(conditional.IfTrue, state);
            var ifFalse = ConvertInternal(conditional.IfFalse, state);
            return $"CASE WHEN {condition} THEN {ifTrue} ELSE {ifFalse} END";
        }

        private static object? CreateObjectFromNewExpression(NewExpression newExpr, ConversionState state)
        {
            var constructorArgs = GetArguments(newExpr.Arguments, state);
            return newExpr.Constructor?.Invoke(constructorArgs);
        }

        private static object?[] GetArguments(IEnumerable<Expression> arguments, ConversionState state)
        {
            List<object?> values = [];
            foreach (var arg in arguments)
            {
                if (arg is ConstantExpression constArg)
                {
                    values.Add(constArg.Value);
                }
                else if (arg is MemberExpression memberArg)
                {
                    values.Add(GetValueFromMember(memberArg, null, state));
                }
                else if (arg is MethodCallExpression methodArg)
                {
                    values.Add(HandleMethodCall(methodArg, state));
                }
                else if (arg is NewExpression newArg)
                {
                    values.Add(CreateObjectFromNewExpression(newArg, state));
                }
                else
                {
                    throw new NotSupportedException($"Argument type {arg.GetType()} is not supported.");
                }
            }
            return [.. values];
        }

        private static string GetSqlOperator(ExpressionType nodeType)
        {
            return nodeType switch
            {
                ExpressionType.AndAlso => "AND",
                ExpressionType.OrElse => "OR",
                ExpressionType.Equal => "=",
                ExpressionType.NotEqual => "<>",
                ExpressionType.GreaterThan => ">",
                ExpressionType.GreaterThanOrEqual => ">=",
                ExpressionType.LessThan => "<",
                ExpressionType.LessThanOrEqual => "<=",
                _ => throw new NotSupportedException($"Operator {nodeType} is not supported.")
            };
        }

        private static object? GetValueFromMember(MemberExpression member, object? instance, ConversionState state)
        {
            if (member.Expression is MemberExpression innerMember)
            {
                instance = GetValueFromMember(innerMember, null, state);
            }
            else if (member.Expression is ConstantExpression constExpr)
            {
                instance = constExpr.Value;
            }
            else if (member.Expression is MethodCallExpression methodCallExpr)
            {
                instance = HandleMethodCall(methodCallExpr, state);
            }

            if (member.Member is PropertyInfo prop)
            {
                return prop.GetValue(instance);
            }
            if (member.Member is FieldInfo field)
            {
                return field.GetValue(instance);
            }

            throw new NotSupportedException("Only properties and fields are supported in method arguments.");
        }

        private static string HandleMemberExpression(MemberExpression member, ConversionState state)
        {
            // Handle static properties (like DateTime.UtcNow)
            return member.Expression switch
            {
                null => AddParameter(GetValueFromMember(member, null, state), state),
                ParameterExpression paramExpr => HandleParameterExpression(paramExpr, member.Member.Name, state),
                ConstantExpression constExpr => AddParameter(GetValueFromMember(member, constExpr.Value, state), state),
                MemberExpression innerMember => AddParameter(GetValueFromMember(member, GetValueFromMember(innerMember, null, state), state), state),
                _ => member.Member.Name
            };
        }

        private static string HandleMethodCall(MethodCallExpression methodCall, ConversionState state)
        {
            object? instance = null;
            if (methodCall.Object is MemberExpression memberExpr)
            {
                instance = GetValueFromMember(memberExpr, null, state);
            }

            var arguments = GetArguments(methodCall.Arguments, state);
            var result = methodCall.Method.Invoke(instance, arguments);
            return AddParameter(result, state);
        }
    }
}
