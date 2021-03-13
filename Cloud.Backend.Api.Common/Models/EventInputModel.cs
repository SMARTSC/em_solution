using System.ComponentModel.DataAnnotations;

namespace EM.Cloud.Backend.Api.Common.Models
{
    public class EventInputModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.Event, System.Guid>
    {
        public System.Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ImageMediaInputModel Logo { get; set; }

        public ImageMediaInputModel Overlay { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public System.DateTime Start { get; set; }

        public System.DateTime End { get; set; }

        public string PlaceName { get; set; }

        public Persistence.Entities.Event Export()
        {
            var entity = new Persistence.Entities.Event();

            Mergue(entity);

            return entity;
        }

        public void Import(Persistence.Entities.Event entity)
        {
            throw new System.NotImplementedException();
        }

        public void Mergue(Persistence.Entities.Event entity)
        {
            entity.Name = Name;
            entity.Description = Description;
            entity.Start = Start;
            entity.End = End;
            entity.PlaceName = PlaceName;
        }
    }
}
