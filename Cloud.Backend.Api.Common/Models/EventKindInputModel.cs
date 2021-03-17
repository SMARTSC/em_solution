namespace EM.Cloud.Backend.Api.Common.Models
{
    public class EventKindInputModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.EventKind, System.Guid>
    {
        public System.Guid Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }

        public Persistence.Entities.EventKind Export()
        {
            var entity = new Persistence.Entities.EventKind();

            Mergue(entity);

            return entity;
        }

        public void Import(Persistence.Entities.EventKind entity)
        {
            throw new System.NotImplementedException();
        }

        public void Mergue(Persistence.Entities.EventKind entity)
        {
            entity.Name = Name;
        }
    }
}
