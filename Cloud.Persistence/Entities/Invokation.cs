using System.ComponentModel.DataAnnotations;

namespace EM.Cloud.Persistence.Entities
{
    public class Invokation : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Entities.IEntity<string>
    {
        public Invokation()
        {
            At = System.DateTime.Now;
        }

        [Key]
        public string Id { get; set; }

        public System.DateTime At { get; set; }

        public string Session { get; set; }

        [Required]
        public string NetAddress { get; set; }

        [Required]
        public string RequestMethod { get; set; }

        [Required]
        public string RequestPath { get; set; }

        public string RouteArea { get; set; }

        public string RouteController { get; set; }

        public string RouteAction { get; set; }

        public Terminal Terminal { get; set; }
    }
}