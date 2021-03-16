using EM.Cloud.Persistence.Entities;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities;

namespace EM.Cloud.Helpers
{
    public class PhotoSessionContentDeletedCascadeHelper
    {
        private readonly IEntityRepository<PhotoSessionContent, System.Guid> photoSessionContentRepository;
        private readonly IEntityRepository<Image, System.Guid> imageRepository;
        private readonly IEntityRepository<Thumbnail, System.Guid> thumbnailRepository;

        public PhotoSessionContentDeletedCascadeHelper(IEntityRepository<PhotoSessionContent, System.Guid> photoSessionContentRepository, IEntityRepository<Image, System.Guid> imageRepository, IEntityRepository<Thumbnail, System.Guid> thumbnailRepository)
        {
            this.photoSessionContentRepository = photoSessionContentRepository;
            this.imageRepository = imageRepository;
            this.thumbnailRepository = thumbnailRepository;
        }

        public void Cascade(PhotoSessionContent target, System.Security.Principal.IIdentity userIdentity, bool isTargetFullLazyLoaded, bool targetDelete, bool commit = false)
        {
            if (!isTargetFullLazyLoaded)
            {
                target = photoSessionContentRepository.Get(target.Id);
            }

            foreach (var bridge in target.ImageBridges)
            {
                var thumbnail = bridge.Image.Thumbnail;

                thumbnailRepository.Delete(thumbnail, userIdentity.Name, false);

                imageRepository.Delete(bridge.Image, userIdentity.Name, false);
            }

            if (targetDelete)
            {
                photoSessionContentRepository.Delete(target, userIdentity.Name, commit);
            }            
        }
    }
}
