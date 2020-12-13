namespace EM.Cloud.Backend.Api.Common.Models
{
    public class PictureContentInputModel : Cloud.Api.Common.Models.ContentInputModel, SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.PictureContent, System.Guid>
    {
        [System.ComponentModel.DataAnnotations.Required]
        public byte[] ContentData { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string ContentType { get; set; }

        public new Persistence.Entities.PictureContent Export()
        {
            var entity = new Persistence.Entities.PictureContent
            {
                Id = Id
            };

            Mergue(entity);

            return entity;
        }

        public void Import(Persistence.Entities.PictureContent entity)
        {
            base.Import(entity);
        }

        public void Mergue(Persistence.Entities.PictureContent entity)
        {
            base.Mergue(entity);
        }
    }
}
