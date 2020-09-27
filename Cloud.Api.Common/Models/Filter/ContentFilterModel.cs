using EM.Cloud.Api.Common.Extensions;

namespace EM.Cloud.Api.Common.Models.Filter
{
    public class ContentFilterModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.Filter.IEntityFilterModel<Persistence.Entities.Content, System.Guid>
    {
        public System.Guid AlbumId { get; set; }

        public System.Collections.Generic.Dictionary<Enumerations.ContentKind, bool> TargetKinds { get; set; }

        public System.Linq.Expressions.Expression<System.Func<Persistence.Entities.Content, bool>> GetSelector()
        {
            return entity => entity.Album.Id == AlbumId && TargetKinds.ContainsKey(entity.ToContentKind()) && TargetKinds[entity.ToContentKind()] && AssertConcreteContentContrains(entity);
        }

        private bool AssertConcreteContentContrains(Persistence.Entities.Content entity)
        {
            if (entity is Persistence.Entities.PhotoSessionContent)
            {
                var photoSessionContent = entity as Persistence.Entities.PhotoSessionContent;

                return photoSessionContent.Album.PhotoSessionCaptureCount <= photoSessionContent.ImageBridges.Count;
            }

            return true;
        }
    }
}
