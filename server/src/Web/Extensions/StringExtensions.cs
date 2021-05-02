namespace Web.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string s)
        {
            return s[..1].ToLower() + s[1..];
        }
    }
}
