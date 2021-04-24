namespace EM.Print.Api.Common.Models
{
    public class PrintOrderInputModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.PrintOrder, long>
    {
        public long Id { get; set; }

        public bool Enabled { get; set; }

        public System.DateTime Created { get; set; }

        public System.DateTime Attended { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public int Count { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public byte[] Data { get; set; }

        public Persistence.Entities.PrintOrder Export()
        {
            var entity = new Persistence.Entities.PrintOrder
            {
                Created = Created,
                Count = Count,
                Data = Data
            };

            Mergue(entity);

            return entity;
        }

        public void Import(Persistence.Entities.PrintOrder entity)
        {
            Id = entity.Id;
            Enabled = entity.Enabled;
            Created = entity.Created;
            Attended = entity.Attended;
            Count = entity.Count;
            Data = entity.Data;
        }

        public void Mergue(Persistence.Entities.PrintOrder entity)
        {
            entity.Enabled = Enabled;
            entity.Attended = Attended;
        }
    }
}
