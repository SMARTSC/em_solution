namespace EM.Terminal.Common.Factories
{
    public static class ThumbnailUriFactory
    {
        public static System.Uri BuildThumbnailUri(string cloudServer, System.Guid thumbnailId)
        {
            return new System.Uri(string.Format("http://{0}/media/default/thumbnail/{1}", cloudServer, thumbnailId));
        }
    }
}