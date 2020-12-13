using System.Linq;

namespace EM.Cloud.Backend.Api.Common.Models
{
    public class EventDisplayModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Event, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string Name { get; set; }

        public Cloud.Api.Common.Models.ImageMediaDisplayModel Logo { get; set; }

        public string Descrition { get; set; }

        public System.DateTime Start { get; set; }

        public System.DateTime End { get; set; }

        public string PlaceName { get; set; }

        public string Annotations { get; set; }

        public AlbumDisplayModel[] Albums { get; set; }

        public void Import(Persistence.Entities.Event entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Descrition = entity.Descrition;
            Start = entity.Start;
            End = entity.End;
            PlaceName = entity.PlaceName;
            Annotations = entity.Annotations;

            Albums = entity.Albums.Select((Persistence.Entities.Album album) =>
            {
                var result = new AlbumDisplayModel();

                result.Import(album);

                return result;
            }).ToArray();

            Logo = new Cloud.Api.Common.Models.ImageMediaDisplayModel();

            Logo.Import(entity.Logo);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
