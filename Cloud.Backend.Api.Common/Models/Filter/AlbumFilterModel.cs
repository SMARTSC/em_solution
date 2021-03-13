namespace EM.Cloud.Backend.Api.Common.Models.Filter
{
    public class AlbumFilterModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.Filter.IEntityFilterModel<Persistence.Entities.Album, System.Guid>
    {
        public System.Guid EventId { get; set; }

        public System.Linq.Expressions.Expression<System.Func<Persistence.Entities.Album, bool>> GetSelector()
        {
            if (EventId == System.Guid.Empty)
            {
                return entity => true;
            }

            return entity => EventId == entity.EventId;
        }
    }
}
