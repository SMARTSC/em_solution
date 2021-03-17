namespace EM.Cloud.Models.View
{
    public class EventDisplayViewModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Event, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string Name { get; set; }

        public System.Guid LogoId { get; set; }

        public System.Guid OverlayId { get; set; }

        public string KindName { get; set; }

        public string Descrition { get; set; }

        public System.DateTime Start { get; set; }

        public System.DateTime End { get; set; }

        public string PlaceName { get; set; }

        public string Annotations { get; set; }

        public void Import(Persistence.Entities.Event entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            LogoId = entity.Logo?.Id ?? System.Guid.Empty;
            OverlayId = entity.Overlay?.Id ?? System.Guid.Empty;
            KindName = entity.Kind.Name;
            Descrition = entity.Description;
            Start = entity.Start;
            End = entity.End;
            PlaceName = entity.PlaceName;
            Annotations = entity.Annotations;
        }
    }
}
