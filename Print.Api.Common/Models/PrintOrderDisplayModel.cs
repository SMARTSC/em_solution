namespace EM.Print.Api.Common.Models
{
    public class PrintOrderDisplayModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.PrintOrder, long>
    {
        public long Id { get; set; }

        public bool Enabled { get; set; }

        public System.DateTime Created { get; set; }

        public System.DateTime Attended { get; set; }

        public int Count { get; set; }

        public byte[] Data { get; set; }

        public void Import(Persistence.Entities.PrintOrder entity)
        {
            Id = entity.Id;
            Enabled = entity.Enabled;
            Created = entity.Created;
            Attended = entity.Attended;
            Count = entity.Count;
            Data = entity.Data;
        }
    }
}
