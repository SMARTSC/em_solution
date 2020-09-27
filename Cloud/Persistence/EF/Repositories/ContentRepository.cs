using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EM.Cloud.Persistence.EF.Repositories
{
    public class ContentRepository : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.Content, System.Guid>
    {
        public ContentRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<Entities.Content> GetQueryableDbSet()
        {
            var contentDbSet = base.GetQueryableDbSet();

            var result = new EnumerableQuery<Entities.Content>(DbContext.Set<Entities.PictureContent>()
                .Include(e => e.Album)
                .Include(e => e.Image).ThenInclude(e => e.Thumbnail));

            result = new EnumerableQuery<Entities.Content>(result.Concat(DbContext.Set<Entities.PhotoSessionContent>()
                .Include(e => e.Album)
                .Include(e => e.ImageBridges).ThenInclude(b => b.Image).ThenInclude(e => e.Thumbnail)));

            result = new EnumerableQuery<Entities.Content>(result.Concat(DbContext.Set<Entities.ClipContent>()
                .Include(e => e.Album)
                .Include(e => e.Video).ThenInclude(e => e.Thumbnail)));

            result = new EnumerableQuery<Entities.Content>(result.Concat(DbContext.Set<Entities.AnimateContent>()
                .Include(e => e.Album)
                .Include(e => e.ImageBridges).ThenInclude(b => b.Image).ThenInclude(e => e.Thumbnail)));

            return result.OrderBy(e => e.Created).AsQueryable();
        }

        public override System.Func<Entities.Content, object> GetPagingOrderSelector()
        {
            return entity => entity.Created;
        }

        public override bool IsPagingOrderDescending()
        {
            return true;
        }
    }
}
