using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EM.Cloud.Persistence.Entities
{
    public class Event : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Entities.INamedEntity<System.Guid>
    {
        public Event()
        {
            Id = System.Guid.NewGuid();

            ContractingClientBridges = new List<ContractingEventClientBridge>();
            PlacingClientBridges = new List<PlacingEventClientBridge>();
            SponsoringClientBridges = new List<SponsoringEventClientBridge>();
        }

        [Key]
        public System.Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image Logo { get; set; }

        //public SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image PrintMiddleLogo { get; set; }

        //public SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image PrintMiddleStrechLogo { get; set; }

        //public SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image PrintSmallLogo { get; set; }

        //public SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image PrintSmallStrechLogo { get; set; }

        [MaxLength(255)]
        public string Descrition { get; set; }

        public System.DateTime Start { get; set; }

        public System.DateTime End { get; set; }

        public string PlaceName { get; set; }

        public string Annotations { get; set; }

        public ICollection<Album> Albums { get; set; }

        public ICollection<ContractingEventClientBridge> ContractingClientBridges { get; set; }

        public ICollection<PlacingEventClientBridge> PlacingClientBridges { get; set; }

        public ICollection<SponsoringEventClientBridge> SponsoringClientBridges { get; set; }
    }
}
