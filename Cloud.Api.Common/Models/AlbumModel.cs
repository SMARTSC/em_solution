namespace EM.Cloud.Api.Common.Models
{
    public class AlbumModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Album, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string Title { get; set; }

        public string Alias { get; set; }

        public bool Enabled { get; set; }

        public string Description { get; set; }

        public EventModel Event { get; set; }

        public void Import(Persistence.Entities.Album entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            Alias = entity.Alias;
            Enabled = entity.Enabled;
            Description = entity.Description;

            Event = new EventModel();

            Event.Import(entity.Event);
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
