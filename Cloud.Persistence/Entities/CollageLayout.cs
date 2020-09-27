using System.ComponentModel.DataAnnotations;

namespace EM.Cloud.Persistence.Entities
{
    public class CollageLayout : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Entities.IHabilitableEntity<System.Guid>
    {
        public CollageLayout()
        {
            Enabled = true;
        }

        [Key]
        public System.Guid Id { get; set; }

        public bool Enabled { get; set; }
    }
}
