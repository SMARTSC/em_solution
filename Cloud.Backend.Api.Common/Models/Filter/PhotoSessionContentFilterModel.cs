namespace EM.Cloud.Backend.Api.Common.Models.Filter
{
    public class PhotoSessionContentFilterModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.Filter.IEntityFilterModel<Persistence.Entities.PhotoSessionContent, System.Guid>
    {
        public System.Guid AlbumId { get; set; }

        public System.Linq.Expressions.Expression<System.Func<Persistence.Entities.PhotoSessionContent, bool>> GetSelector()
        {
            return entity => entity.Album.Id == AlbumId;
        }
    }
}
