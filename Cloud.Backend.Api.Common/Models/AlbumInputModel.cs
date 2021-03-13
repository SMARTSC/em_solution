using System.ComponentModel.DataAnnotations;

namespace EM.Cloud.Backend.Api.Common.Models
{
    public class AlbumInputModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.Album, System.Guid>
    {
        public System.Guid Id { get; set; }

        public System.Guid EventId { get; set; }

        public bool Enabled { get; set; }

        public bool Public { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Alias { get; set; }

        public int PhotoSessionCaptureCount { get; set; }

        public System.Guid DefaultPictureContentCollageLayoutId { get; set; }

        public System.Guid DefaultPhotoSessionContentCollageLayoutId { get; set; }

        public ImageMediaInputModel Overlay { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public Persistence.Entities.Album Export()
        {
            var entity = new Persistence.Entities.Album();

            Mergue(entity);

            return entity;
        }

        public void Import(Persistence.Entities.Album entity)
        {
            throw new System.NotImplementedException();
        }

        public void Mergue(Persistence.Entities.Album entity)
        {
            entity.Enabled = Enabled;
            entity.Public = Public;
            entity.Title = Title;
            entity.Alias = Alias;
            entity.PhotoSessionCaptureCount = PhotoSessionCaptureCount;
            entity.Description = Description;
        }
    }
}
