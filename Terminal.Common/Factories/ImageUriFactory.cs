namespace EM.Terminal.Common.Factories
{
    public static class ImageUriFactory
    {
        public static System.Uri BuildImageUri(string cloudServer, System.Guid imageId)
        {
            return new System.Uri(string.Format("http://{0}/media/default/image/{1}", cloudServer, imageId));
        }
    }
}
