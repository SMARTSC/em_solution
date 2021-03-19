namespace EM.Cloud.Api.Common.Models
{
    public class EventModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Event, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string Name { get; set; }

        public string KindName { get; set; }

        public ImageMediaDisplayModel Logo { get; set; }

        public ImageMediaDisplayModel Overlay { get; set; }

        public string Description { get; set; }

        public System.DateTime Start { get; set; }

        public System.DateTime End { get; set; }

        public string PlaceName { get; set; }

        public string Annotations { get; set; }

        public void Import(Persistence.Entities.Event entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            KindName = entity.Kind.Name;
            Description = entity.Description;
            Start = entity.Start;
            End = entity.End;
            PlaceName = entity.PlaceName;
            Annotations = entity.Annotations;

            Logo = new ImageMediaDisplayModel();

            Logo.Import(entity.Logo);

            if (entity.Overlay != null)
            {
                Overlay = new ImageMediaDisplayModel();

                Overlay.Import(entity.Overlay);
            }
        }
    }
}
