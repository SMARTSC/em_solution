namespace EM.Terminal.Common.Factories
{
    public static class UriFactory
    {
        public static System.Uri BuildAlbumImageThumbnailUri(System.Guid albumId, System.Guid thumbnailId)
        {
            return new System.Uri(string.Format("{0}://{1}/business/media/album/{2}/thumbnail/{3}", Providers.ConstantProvider.CLOUD_CONNECTION_SCHEMA, Providers.ConstantProvider.CLOUD_SERVER_HOST, albumId, thumbnailId));
        }

        public static System.Uri BuildImageUri(System.Guid imageId)
        {
            return new System.Uri(string.Format("{0}://{1}/business/media/image/{2}", Providers.ConstantProvider.CLOUD_CONNECTION_SCHEMA, Providers.ConstantProvider.CLOUD_SERVER_HOST, imageId));
        }

        public static System.Uri BuildImageThumbnailUri(System.Guid thumbnailId)
        {
            return new System.Uri(string.Format("{0}://{1}/business/media/thumbnail/{2}", Providers.ConstantProvider.CLOUD_CONNECTION_SCHEMA, Providers.ConstantProvider.CLOUD_SERVER_HOST, thumbnailId));
        }
    }
}
