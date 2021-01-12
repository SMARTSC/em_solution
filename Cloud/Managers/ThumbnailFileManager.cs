using EM.Cloud.Extensions;
using SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities;

namespace EM.Cloud.Managers
{
    public class ThumbnailFileManager : SmartSolucionesCuba.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IThumbnailFileManagerFacade
    {
        public void DeleteThumbnailFile(Thumbnail thumbnail)
        {
            Providers.ThumbnailsBlobContainerClientProvider.Instance.GetBlobContainerClient().DeleteBlobIfExists(thumbnail.Id.ToBlobId(thumbnail.ContentType));
        }

        public string GetThumbnailAbsoluteFilePath(Thumbnail thumbnail)
        {
            throw new System.InvalidOperationException();
        }

        public byte[] GetThumbnailFileContent(Thumbnail thumbnail)
        {
            var blobClient = Providers.ThumbnailsBlobContainerClientProvider.Instance.GetBlobContainerClient().GetBlobClient(thumbnail.Id.ToBlobId(thumbnail.ContentType));

            var azureResponse = blobClient.Download();

            var resultStream = new System.IO.MemoryStream();

            azureResponse.Value.Content.CopyTo(resultStream);

            return resultStream.ToArray();
        }

        public string GetThumbnailRelativeFilePath(Thumbnail thumbnail)
        {
            throw new System.InvalidOperationException();
        }

        public void SaveThumbnailFileContent(System.Guid thumbnailId, byte[] content, string contentType)
        {
            Providers.ThumbnailsBlobContainerClientProvider.Instance.GetBlobContainerClient().UploadBlob(thumbnailId.ToBlobId(contentType), new System.IO.MemoryStream(content));
        }
    }
}
