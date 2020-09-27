namespace EM.Cloud.Api.Common.Models
{
    public class EventInputModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.Event, System.Guid>
    {
        public System.Guid Id { get; set; }

        public Persistence.Entities.Event Export()
        {
            throw new System.NotImplementedException();
        }

        public void Import(Persistence.Entities.Event entity)
        {
            throw new System.NotImplementedException();
        }

        public void Mergue(Persistence.Entities.Event entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
