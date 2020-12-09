using EM.Cloud.Extensions;
using SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities;

namespace EM.Cloud.Managers
{
    public class ImageFileManager : SmartSolucionesCuba.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IImageFileManagerFacade
    {
        public void DeleteImageFile(Image image)
        {
            Providers.ImagesBlobContainerClientProvider.Instance.GetBlobContainerClient().DeleteBlobIfExists(GetBlobId(image.Id, image.ContentType));
        }

        public string GetImageAbsoluteFilePath(Image image)
        {
            throw new System.InvalidOperationException();
        }

        public byte[] GetImageFileContent(Image image)
        {
            var blobClient = Providers.ImagesBlobContainerClientProvider.Instance.GetBlobContainerClient().GetBlobClient(GetBlobId(image.Id, image.ContentType));

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
            Providers.ImagesBlobContainerClientProvider.Instance.GetBlobContainerClient().UploadBlob(GetBlobId(imageId, contentType), new System.IO.MemoryStream(content));
        }

        private string GetBlobId(System.Guid id, string contentType)
        {
            return string.Format("{0}.{1}", id, contentType.ContentTypeToExtension());
        }
    }
}
