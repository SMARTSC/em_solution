using Microsoft.EntityFrameworkCore;

namespace EM.Cloud.Persistence.EF.Repositories
{
    public class AlbumRepository : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.Album, System.Guid>
    {
        public AlbumRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override System.Linq.IQueryable<Entities.Album> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet()
                .Include(entity => entity.LockBridges)
                .Include(entity => entity.CollageLayoutBridges).ThenInclude(bridge => bridge.CollageLayout)
                .Include(entity => entity.DefaultPhotoSessionContentCollageLayout)
                .Include(entity => entity.DefaultPictureContentCollageLayout);
        }
    }
}
