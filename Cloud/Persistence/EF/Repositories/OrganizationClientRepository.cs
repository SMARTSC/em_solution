using Microsoft.EntityFrameworkCore;

namespace EM.Cloud.Persistence.EF.Repositories
{
    public class OrganizationClientRepository : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.OrganizationClient, System.Guid>
    {
        public OrganizationClientRepository(DbContext dbContext) : base(dbContext)
        {
        }
        protected override System.Linq.IQueryable<Entities.OrganizationClient> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet()
                .Include(entity => entity.Identity).ThenInclude(entity => entity.Image).ThenInclude(entity => entity.Thumbnail)
                .Include(entity => entity.Identity).ThenInclude(entity => entity.OrganizationWebSiteContactItemBridges).ThenInclude(entity => entity.ContactItem)
                .Include(entity => entity.Identity).ThenInclude(entity => entity.OrganizationSocialContactItemBridges).ThenInclude(entity => entity.ContactItem)
                .Include(entity => entity.Identity).ThenInclude(entity => entity.OrganizationPostalContactItemBridges).ThenInclude(entity => entity.ContactItem)
                .Include(entity => entity.Identity).ThenInclude(entity => entity.OrganizationPhoneContactItemBridges).ThenInclude(entity => entity.ContactItem)
                .Include(entity => entity.Identity).ThenInclude(entity => entity.OrganizationEmailContactItemBridges).ThenInclude(entity => entity.ContactItem)
                .Include(entity => entity.Identity).ThenInclude(entity => entity.Memberships).ThenInclude(entity => entity.Person);
        }
    }
}
