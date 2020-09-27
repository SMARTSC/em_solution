using EM.Cloud.Extensions;
using Microsoft.Extensions.Configuration;
using SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities;

namespace EM.Cloud.Managers
{
    public class ImageFileManager : SmartSolucionesCuba.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IImageFileManagerFacade
    {
        private readonly Azure.Storage.Blobs.BlobContainerClient blobStorageContainerClient;

        public ImageFileManager(IConfiguration configuration)
        {
            blobStorageContainerClient = new Azure.Storage.Blobs.BlobContainerClient(configuration.GetConnectionString("BlobStorageConnection"), "media-images");

            if (!blobStorageContainerClient.Exists())
            {
                blobStorageContainerClient.Create();

                blobStorageContainerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            }
        }

        public void DeleteImageFile(Image image)
        {
            blobStorageContainerClient.DeleteBlobIfExists(GetBlobId(image.Id, image.ContentType));
        }

        public string GetImageAbsoluteFilePath(Image image)
        {
            throw new System.InvalidOperationException();
        }

        public byte[] GetImageFileContent(Image image)
        {
            var blobClient = blobStorageContainerClient.GetBlobClient(GetBlobId(image.Id, image.ContentType));

            var azureResponse = blobClient.Download();

            var resultStream = new System.IO.MemoryStream();

            azureResponse.Value.Content.CopyTo(resultStream);

            return resultStream.ToArray();
        }

        public string GetImageRelativeFilePath(Image image)
        {
            throw new System.InvalidOperationException();
        }

        public void SaveImageFileContent(System.Guid imageId, byte[] content, string contentType)
        {
            blobStorageContainerClient.UploadBlob(GetBlobId(imageId, contentType), new System.IO.MemoryStream(content));
        }

        private string GetBlobId(System.Guid id, string contentType)
        {
            return string.Format("{0}.{1}", id, contentType.ContentTypeToExtension());
        }
    }
}
