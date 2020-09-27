namespace EM.Cloud.Persistence.EF.Repositories
{
    public class PrintOrderRepository : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.PrintOrder, long>
    {
        public PrintOrderRepository(Microsoft.EntityFrameworkCore.DbContext dbContext) : base(dbContext)
        {
        }

        public override System.Func<Entities.PrintOrder, object> GetPagingOrderSelector()
        {
            return entity => entity.Created;
        }
    }
}
