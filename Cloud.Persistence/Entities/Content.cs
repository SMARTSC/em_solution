using System.ComponentModel.DataAnnotations;

namespace EM.Cloud.Persistence.Entities
{
    public abstract class Content : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Entities.IHabilitableEntity<System.Guid>
    {
        public Content()
        {
            Id = System.Guid.NewGuid();
            Created = System.DateTime.Now;
        }

        [Key]
        public System.Guid Id { get; set; }

        public bool Enabled { get; set; }

        [Required]
        public System.DateTime Created { get; set; }

        public string Title { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public Album Album { get; set; }
    }
}
