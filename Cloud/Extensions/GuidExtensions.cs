namespace EM.Cloud.Extensions
{
    public static class GuidExtensions
    {
        public static string ToBlobId(this System.Guid target, string contentType)
        {
            return string.Format("{0}.{1}", target, contentType.ContentTypeToExtension());
        }
    }
}
