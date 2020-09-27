namespace EM.Cloud.Persistence.Entities
{
    public class PrintOrder : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Entities.IHabilitableEntity<long>
    {
        public PrintOrder()
        {
            Created = System.DateTime.Now;
            Created = System.DateTime.MinValue;
        }

        [System.ComponentModel.DataAnnotations.Key]
        public long Id { get; set; }

        public bool Enabled { get; set; }

        public System.DateTime Created { get; set; }

        public System.DateTime Attended { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public int Count { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public byte[] Data { get; set; }
    }
}
