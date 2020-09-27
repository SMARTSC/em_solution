using Microsoft.EntityFrameworkCore;

namespace EM.Cloud.Persistence.EF.Repositories
{
    public class TerminalRepository : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.Terminal, string>
    {
        public TerminalRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override System.Linq.IQueryable<Entities.Terminal> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet().Include(e => e.AlbumBridges);
        }
    }
}
