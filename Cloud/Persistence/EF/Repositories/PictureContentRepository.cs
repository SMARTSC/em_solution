using Microsoft.EntityFrameworkCore;

namespace EM.Cloud.Persistence.EF.Repositories
{
    public class PictureContentRepository : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.PictureContent, System.Guid>
    {
        public PictureContentRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override System.Linq.IQueryable<Entities.PictureContent> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet().Include(entity => entity.Image).ThenInclude(entity => entity.Thumbnail);
        }
    }
}
