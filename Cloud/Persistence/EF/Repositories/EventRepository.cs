using Microsoft.EntityFrameworkCore;

namespace EM.Cloud.Persistence.EF.Repositories
{
    public class EventRepository : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.Event, System.Guid>
    {
        public EventRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override System.Linq.IQueryable<Entities.Event> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet()
                .Include(entity => entity.Logo)
                .Include(entity => entity.Overlay).ThenInclude(entity => entity.Thumbnail)
                //.Include(entity => entity.PrintMiddleLogo)
                //.Include(entity => entity.PrintMiddleStrechLogo)
                //.Include(entity => entity.PrintSmallLogo)
                //.Include(entity => entity.PrintSmallStrechLogo)
                .Include(entity => entity.Albums).ThenInclude(entity => entity.DefaultPictureContentCollageLayout)
                .Include(entity => entity.Albums).ThenInclude(entity => entity.LockBridges)
                .Include(entity => entity.Albums).ThenInclude(entity => entity.DefaultPhotoSessionContentCollageLayout)
                .Include(entity => entity.Albums).ThenInclude(entity => entity.CollageLayoutBridges).ThenInclude(bridge => bridge.CollageLayout);
        }

        public override System.Func<Entities.Event, object> GetPagingOrderSelector()
        {
            return entity => entity.Start;
        }

        public override bool IsPagingOrderDescending()
        {
            return true;
        }
    }
}
