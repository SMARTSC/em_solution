using Microsoft.EntityFrameworkCore;

namespace EM.Cloud.Persistence.EF.Repositories
{
    public class PersonClientRespository : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.PersonClient, System.Guid>
    {
        public PersonClientRespository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override System.Linq.IQueryable<Entities.PersonClient> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet()
                .Include(entity => entity.Identity).ThenInclude(entity => entity.Image).ThenInclude(entity => entity.Thumbnail)
                .Include(entity => entity.Identity).ThenInclude(entity => entity.PersonWebSiteContactItemBridges).ThenInclude(entity => entity.ContactItem)
                .Include(entity => entity.Identity).ThenInclude(entity => entity.PersonSocialContactItemBridges).ThenInclude(entity => entity.ContactItem)
                .Include(entity => entity.Identity).ThenInclude(entity => entity.PersonPostalContactItemBridges).ThenInclude(entity => entity.ContactItem)
                .Include(entity => entity.Identity).ThenInclude(entity => entity.PersonPhoneContactItemBridges).ThenInclude(entity => entity.ContactItem)
                .Include(entity => entity.Identity).ThenInclude(entity => entity.PersonEmailContactItemBridges).ThenInclude(entity => entity.ContactItem)
                .Include(entity => entity.Identity).ThenInclude(entity => entity.Memberships).ThenInclude(entity => entity.Organization);
        }
    }
}
