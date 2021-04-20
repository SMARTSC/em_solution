using Microsoft.EntityFrameworkCore;

namespace EM.Print.Cloud.Persistence.EF.Repositories
{
    public class PrintOrderRepository : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Print.Persistence.Entities.PrintOrder, long>
    {
        public PrintOrderRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public override System.Func<Print.Persistence.Entities.PrintOrder, object> GetPagingOrderSelector()
        {
            return entity => entity.Created;
        }
    }
}
