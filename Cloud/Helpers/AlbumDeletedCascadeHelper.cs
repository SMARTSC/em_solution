using EM.Cloud.Persistence.Entities;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities;
using SmartSolucionesCuba.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade;

namespace EM.Cloud.Helpers
{
    public class AlbumDeletedCascadeHelper
    {
        private readonly IEntityRepository<Album, System.Guid> albumRepository;
        private readonly IEntityRepository<Image, System.Guid> imageRepository;
        private readonly IEntityRepository<Thumbnail, System.Guid> thumbnailRepository;
        private readonly IImageFileManagerFacade imageFileManager;
        private readonly IThumbnailFileManagerFacade thumbnailFileManager;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;
        private readonly PictureContentDeletedCascadeHelper pictureContentDeletedCascadeHelper;
        private readonly PhotoSessionContentDeletedCascadeHelper photoSessionContentDeletedCascadeHelper;

        public AlbumDeletedCascadeHelper(PhotoSessionContentDeletedCascadeHelper photoSessionContentDeletedCascadeHelper, PictureContentDeletedCascadeHelper pictureContentDeletedCascadeHelper, IEntityRepository<Image, System.Guid> imageRepository, IEntityRepository<Thumbnail, System.Guid> thumbnailRepository, IImageFileManagerFacade imageFileManager, IThumbnailFileManagerFacade thumbnailFileManager, Microsoft.Extensions.Configuration.IConfiguration configuration, IEntityRepository<Album, System.Guid> albumRepository)
        {
            this.albumRepository = albumRepository;
            this.imageRepository = imageRepository;
            this.thumbnailRepository = thumbnailRepository;
            this.imageFileManager = imageFileManager;
            this.thumbnailFileManager = thumbnailFileManager;
            this.configuration = configuration;
            this.pictureContentDeletedCascadeHelper = pictureContentDeletedCascadeHelper;
            this.photoSessionContentDeletedCascadeHelper = photoSessionContentDeletedCascadeHelper;
        }

        public void Cascade(Album target, System.Security.Principal.IIdentity userIdentity, bool isTargetFullLazyLoaded, bool targetDelete, bool commit)
        {
            if (!isTargetFullLazyLoaded)
            {
                target = albumRepository.Get(target.Id);
            }

            if (targetDelete)
            {
                albumRepository.Delete(target, userIdentity.Name, false);
            }

            foreach (var content in target.Contents)
            {
                if (content is PictureContent)
                {
                    pictureContentDeletedCascadeHelper.Cascade(content as PictureContent, userIdentity, false, true, false);

                    continue;
                }

                if (content is PhotoSessionContent)
                {

                    photoSessionContentDeletedCascadeHelper.Cascade(content as PhotoSessionContent, userIdentity, false, true, false);
                }
            }

            Providers.ImagesBlobContainerClientProvider.Instance.TargetAlbumBlobContainer(configuration, target.Id);
            Providers.ThumbnailsBlobContainerClientProvider.Instance.TargetAlbumBlobContainer(configuration, target.Id);

            var albumContentImagesBlobContainerClient = Providers.ImagesBlobContainerClientProvider.Instance.GetBlobContainerClient();
            var albumContentThumbnailsBlobContainerClient = Providers.ThumbnailsBlobContainerClientProvider.Instance.GetBlobContainerClient();

            albumContentImagesBlobContainerClient.Delete();
            albumContentThumbnailsBlobContainerClient.Delete();

            Providers.ImagesBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);
            Providers.ThumbnailsBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);

            if (target.Overlay == null) return;

            var overlayThumbnail = target.Overlay.Thumbnail;

            thumbnailRepository.Delete(overlayThumbnail, userIdentity.Name, false);

            thumbnailFileManager.DeleteThumbnailFile(overlayThumbnail);

            imageRepository.Delete(target.Overlay, userIdentity.Name, commit);

            imageFileManager.DeleteImageFile(target.Overlay);
        }
    }
}
