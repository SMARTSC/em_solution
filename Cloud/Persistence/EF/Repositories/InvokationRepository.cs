using Microsoft.EntityFrameworkCore;

namespace EM.Cloud.Persistence.EF.Repositories
{
    public class InvokationRepository : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.Invokation, string>
    {
        public InvokationRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override System.Linq.IQueryable<Entities.Invokation> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet().Include(e => e.Terminal);
        }
    }
}
