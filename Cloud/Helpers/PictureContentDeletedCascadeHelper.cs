using EM.Cloud.Persistence.Entities;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities;

namespace EM.Cloud.Helpers
{
    public class PictureContentDeletedCascadeHelper
    {
        private readonly IEntityRepository<PictureContent, System.Guid> pictureContentRepository;
        private readonly IEntityRepository<Image, System.Guid> imageRepository;
        private readonly IEntityRepository<Thumbnail, System.Guid> thumbnailRepository;

        public PictureContentDeletedCascadeHelper(IEntityRepository<PictureContent, System.Guid> pictureContentRepository, IEntityRepository<Image, System.Guid> imageRepository, IEntityRepository<Thumbnail, System.Guid> thumbnailRepository)
        {
            this.pictureContentRepository = pictureContentRepository;
            this.imageRepository = imageRepository;
            this.thumbnailRepository = thumbnailRepository;
        }

        public void Cascade(PictureContent target, System.Security.Principal.IIdentity userIdentity, bool isTargetFullLazyLoaded, bool targetDelete, bool commit = false)
        {
            if (!isTargetFullLazyLoaded)
            {
                target = pictureContentRepository.Get(target.Id);
            }

            if (targetDelete)
            {
                pictureContentRepository.Delete(target, userIdentity.Name, false);
            }

            var thumbnail = target.Image.Thumbnail;

            thumbnailRepository.Delete(thumbnail, userIdentity.Name, false);

            imageRepository.Delete(target.Image, userIdentity.Name, commit);
        }
    }
}
