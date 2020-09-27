namespace EM.Cloud.Api.Common.Models
{
    public class ContentDisplayModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Content, System.Guid>
    {
        public System.Guid Id { get; set; }

        public bool Enabled { get; set; }

        public System.DateTime Created { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Enumerations.ContentKind Kind { get; set; }

        public virtual void Import(Persistence.Entities.Content entity)
        {
            Id = entity.Id;
            Enabled = entity.Enabled;
            Created = entity.Created;
            Title = entity.Title;
            Description = entity.Description;
        }
    }
}
