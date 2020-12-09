using Microsoft.Extensions.Configuration;

namespace EM.Cloud.Providers
{
    public sealed class ImagesBlobContainerClientProvider
    {
        private static ImagesBlobContainerClientProvider instance;

        private System.Guid? targetAlbumId;

        private Azure.Storage.Blobs.BlobContainerClient blobContainerClient;

        public static ImagesBlobContainerClientProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ImagesBlobContainerClientProvider();
                }

                return instance;
            }
        }

        public ImagesBlobContainerClientProvider()
        {
            targetAlbumId = System.Guid.Empty;
        }

        public Azure.Storage.Blobs.BlobContainerClient GetBlobContainerClient()
        {
            return blobContainerClient;
        }

        public void TargetDefaultBlobContainer(IConfiguration configuration)
        {
            if (targetAlbumId == null) return;

            blobContainerClient = new Azure.Storage.Blobs.BlobContainerClient(configuration.GetConnectionString("BlobStorageConnection"), "media-images");

            if (!blobContainerClient.Exists())
            {
                blobContainerClient.Create();

                blobContainerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            }

            targetAlbumId = null;
        }

        public void TargetAlbumBlobContainer(IConfiguration configuration, System.Guid albumId)
        {
            if (targetAlbumId != null && targetAlbumId.Value == albumId) return;

            blobContainerClient = new Azure.Storage.Blobs.BlobContainerClient(configuration.GetConnectionString("BlobStorageConnection"), string.Format("{0}-media-images", albumId));

            if (!blobContainerClient.Exists())
            {
                blobContainerClient.Create();

                blobContainerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            }

            targetAlbumId = albumId;
        }
    }
}
