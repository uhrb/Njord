namespace Njord.Ais.Extensions.Types
{
    internal static class ASCIIStringsExtensions
    {

        public static bool IsUnavailiableOrNullEmpty(this string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }

            return str.All(_ => _ == '@');
        }
    }
}
