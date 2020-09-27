namespace EM.Cloud.Api.Common.Models
{
    public class ContentInputModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.Content, System.Guid>
    {
        public System.Guid Id { get; set; }

        public bool Enabled { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public System.DateTime Created { get; set; }

        public string Title { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLength(255)]
        public string Description { get; set; }

        public System.Guid AlbumId { get; set; }

        public Persistence.Entities.Content Export()
        {
            throw new System.NotImplementedException();
        }

        public void Import(Persistence.Entities.Content entity)
        {
            Id = entity.Id;
            Enabled = entity.Enabled;
            Created = entity.Created;
            Title = entity.Title;
            Description = entity.Description;
        }

        public void Mergue(Persistence.Entities.Content entity)
        {
            entity.Enabled = Enabled;
            entity.Created = Created;
            entity.Title = Title;
            entity.Description = Description;
        }
    }
}
