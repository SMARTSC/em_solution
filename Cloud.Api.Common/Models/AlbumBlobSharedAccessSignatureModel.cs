namespace EM.Cloud.Api.Common.Models
{
    public class AlbumBlobSharedAccessSignatureModel
    {
        public System.Uri ImagesBlobContainerUri { get; set; }

        public System.Uri ThumbnailsBlobContainerUri { get; set; }

        public System.DateTimeOffset ExpiresOn { get; set; }
    }
}
