namespace EM.Cloud.Backend.Api.Common.Models
{
    public class EventKindDisplayModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.EventKind, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string Name { get; set; }

        public void Import(Persistence.Entities.EventKind entity)
        {
            Id = entity.Id;
            Name = entity.Name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EventKindDisplayModel)) return false;

            var target = obj as EventKindDisplayModel;

            return Id.Equals(target.Id);
        }

        public override int GetHashCode()
        {
            var hashCode = -1919740922;

            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + System.Collections.Generic.EqualityComparer<string>.Default.GetHashCode(Name);

            return hashCode;
        }
    }
}
