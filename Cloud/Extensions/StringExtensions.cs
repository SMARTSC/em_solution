namespace EM.Cloud.Extensions
{
    public static class StringExtensions
    {
        public static string ContentTypeToExtension(this string source)
        {
            switch (source)
            {
                case "image/jpeg":
                    return "jpg";

                default:
                    return source.Split('/')[1].ToLowerInvariant();
            }
        }
    }
}
