namespace EM.Cloud.Api.Common.Models
{
    public class PictureContentDisplayModel : ContentDisplayModel, SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.PictureContent, System.Guid>
    {
        public ImageMediaDisplayModel Image { get; set; }

        public void Import(Persistence.Entities.PictureContent entity)
        {
            Kind = Enumerations.ContentKind.Picture;

            base.Import(entity);

            Image = new ImageMediaDisplayModel();

            Image.Import(entity.Image);
        }
    }
}
