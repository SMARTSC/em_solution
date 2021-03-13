namespace EM.Terminal.Dashboard.Factories
{
    public static class UriFactory
    {
        public static System.Uri BuildAlbumImageThumbnailUri(System.Guid albumId, System.Guid thumbnailId)
        {
            return new System.Uri(string.Format("{1}://{0}/business/media/album/{2}/thumbnail/{3}", Managers.SessionManager.Instance.Server, Managers.SessionManager.CLOUD_CONNECTION_SCHEMA, albumId, thumbnailId));
        }

        public static System.Uri BuildImageUri(System.Guid imageId)
        {
            return new System.Uri(string.Format("{1}://{0}/business/media/image/{2}", Managers.SessionManager.Instance.Server, Managers.SessionManager.CLOUD_CONNECTION_SCHEMA, imageId));
        }

        public static System.Uri BuildImageThumbnailUri(System.Guid thumbnailId)
        {
            return new System.Uri(string.Format("{1}://{0}/business/media/thumbnail/{2}", Managers.SessionManager.Instance.Server, Managers.SessionManager.CLOUD_CONNECTION_SCHEMA, thumbnailId));
        }
    }
}
