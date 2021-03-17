namespace EM.Cloud.Persistence.EF.Repositories
{
    public class EventKindRepository : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.EventKind, System.Guid>
    {
        public EventKindRepository(Microsoft.EntityFrameworkCore.DbContext dbContext) : base(dbContext)
        {
        }

        public override System.Func<Entities.EventKind, object> GetPagingOrderSelector()
        {
            return entity => entity.Name;
        }
    }
}
