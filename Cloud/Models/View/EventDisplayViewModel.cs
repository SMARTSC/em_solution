namespace EM.Cloud.Models.View
{
    public class EventDisplayViewModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Event, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string Name { get; set; }

        public System.Guid LogoId { get; set; }

        //public System.Guid PrintLogoMiddleId { get; set; }

        //public System.Guid PrintMiddleStrechLogoId { get; set; }

        //public System.Guid PrintLogoSmallId { get; set; }

        //public System.Guid PrintSmallStrechLogoId { get; set; }

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
            //PrintLogoMiddleId = entity.PrintMiddleLogo?.Id ?? System.Guid.Empty;
            //PrintMiddleStrechLogoId = entity.PrintMiddleStrechLogo?.Id ?? System.Guid.Empty;
            //PrintLogoSmallId = entity.PrintSmallLogo?.Id ?? System.Guid.Empty;
            //PrintSmallStrechLogoId = entity.PrintSmallStrechLogo?.Id ?? System.Guid.Empty;
            Descrition = entity.Description;
            Start = entity.Start;
            End = entity.End;
            PlaceName = entity.PlaceName;
            Annotations = entity.Annotations;
        }
    }
}
