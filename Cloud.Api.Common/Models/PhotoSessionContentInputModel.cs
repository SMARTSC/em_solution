namespace EM.Cloud.Api.Common.Models
{
    public class PhotoSessionContentInputModel : ContentInputModel, SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.PhotoSessionContent, System.Guid>
    {
        [System.ComponentModel.DataAnnotations.Required]
        public int CaptureCount { get; set; }

        public void Import(Persistence.Entities.PhotoSessionContent entity)
        {
            base.Import(entity);
        }

        public void Mergue(Persistence.Entities.PhotoSessionContent entity)
        {
            base.Mergue(entity);
        }

        public new Persistence.Entities.PhotoSessionContent Export()
        {
            var entity = new Persistence.Entities.PhotoSessionContent
            {
                Id = Id
            };

            Mergue(entity);

            return entity;
        }
    }
}
