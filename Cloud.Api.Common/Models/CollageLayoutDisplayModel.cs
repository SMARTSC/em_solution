namespace EM.Cloud.Api.Common.Models
{
    public class CollageLayoutDisplayModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.CollageLayout, System.Guid>
    {
        public System.Guid Id { get; set; }

        public bool Enabled { get; set; }

        public void Import(Persistence.Entities.CollageLayout entity)
        {
            Id = entity.Id;
            Enabled = entity.Enabled;
        }
    }
}
