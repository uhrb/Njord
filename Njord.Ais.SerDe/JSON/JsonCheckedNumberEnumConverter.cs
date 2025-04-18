using System.Text.Json;
using System.Text.Json.Serialization;

namespace Njord.Ais.SerDe.JSON
{
    public class JsonCheckedNumberEnumConverter<T> : JsonConverter<T> where T : Enum
    {
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetInt32();
            return ConvertWithCheck(value);
        }

        public static T ConvertWithCheck(int value)
        {
            var enumType = typeof(T);
            var under = Enum.GetUnderlyingType(enumType);
            try
            {
                var result = Convert.ChangeType(value, under);
                if (Enum.IsDefined(enumType, result))
                {
                    return (T)Enum.ToObject(enumType, result);
                }
            }
            catch
            {
                // supress
            }

            throw new JsonException($"{enumType.Name} do not contain value of {value}");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(Convert.ToInt32(value));
        }
    }
}
