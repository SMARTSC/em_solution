using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EM.Cloud.Persistence.EF.Repositories
{
    public class ClientRepository : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.Client, System.Guid>
    {
        public ClientRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<Entities.Client> GetQueryableDbSet()
        {
            var result = new EnumerableQuery<Entities.Client>(base.GetQueryableDbSet().Cast<Entities.PersonClient>().Include(e => e.Identity).ThenInclude(e => e.Image).ThenInclude(e => e.Thumbnail));

            result = new EnumerableQuery<Entities.Client>(result.Concat(base.GetQueryableDbSet().Cast<Entities.OrganizationClient>().Include(e => e.Identity).ThenInclude(e => e.Image).ThenInclude(e => e.Thumbnail)));

            return result;
        }
    }
}
