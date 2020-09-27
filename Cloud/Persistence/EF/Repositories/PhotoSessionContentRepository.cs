using Microsoft.EntityFrameworkCore;

namespace EM.Cloud.Persistence.EF.Repositories
{
    public class PhotoSessionContentRepository : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.PhotoSessionContent, System.Guid>
    {
        public PhotoSessionContentRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override System.Linq.IQueryable<Entities.PhotoSessionContent> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet()
                .Include(entity => entity.ImageBridges).ThenInclude(entity => entity.Image).ThenInclude(entity => entity.Thumbnail);
        }
    }
}
