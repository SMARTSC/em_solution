namespace EM.Cloud.Backend.Api.Common.Models.Filter
{
    public class PictureContentFilterModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.Filter.IEntityFilterModel<Persistence.Entities.PictureContent, System.Guid>
    {
        public System.Guid AlbumId { get; set; }

        public System.Linq.Expressions.Expression<System.Func<Persistence.Entities.PictureContent, bool>> GetSelector()
        {
            return entity => entity.Album.Id == AlbumId;
        }
    }
}
