namespace EM.Cloud.Persistence.Entities
{
    public abstract class Client : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Entities.IEntity<System.Guid>
    {
        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid Id { get; set; }

        public string Notes { get; set; }
    }
}