namespace EM.Cloud.Api.Common.Models
{
    public class PhotoSessionContentDisplayModel : ContentDisplayModel, SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.PhotoSessionContent, System.Guid>
    {
        public int CaptureCount { get; set; }

        public PhotoSessionContentDisplayModel()
        {
            Images = new System.Collections.Generic.List<ImageMediaDisplayModel>();
        }

        public System.Collections.Generic.List<ImageMediaDisplayModel> Images { get; set; }

        public void Import(Persistence.Entities.PhotoSessionContent entity)
        {
            Kind = Enumerations.ContentKind.Session;

            base.Import(entity);

            foreach (var bridge in entity.ImageBridges)
            {
                var image = new ImageMediaDisplayModel();

                image.Import(bridge.Image);

                Images.Add(image);
            }
        }
    }
}
