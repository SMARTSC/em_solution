namespace EM.Cloud.Api.Common.Models
{
    public class ImageMediaDisplayModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image, System.Guid>
    {
        public System.Guid Id { get; set; }

        public System.Guid ThumbnailId { get; set; }

        public void Import(SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image entity)
        {
            Id = entity.Id;
            ThumbnailId = entity.Thumbnail?.Id ?? System.Guid.Empty;
        }
    }
}
