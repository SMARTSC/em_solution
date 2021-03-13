using System.Linq;

namespace EM.Cloud.Backend.Api.Common.Models
{
    public class AlbumDisplayModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Album, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string Title { get; set; }

        public string Alias { get; set; }

        public bool Enabled { get; set; }

        public bool Public { get; set; }

        public int PhotoSessionCaptureCount { get; set; }

        public CollageLayoutDisplayModel DefaultPictureContentCollageLayout { get; set; }

        public CollageLayoutDisplayModel DefaultPhotoSessionContentCollageLayout { get; set; }

        public CollageLayoutDisplayModel[] ExtraCollageLayouts;

        public Cloud.Api.Common.Models.ImageMediaDisplayModel Overlay { get; set; }

        public string Description { get; set; }

        public void Import(Persistence.Entities.Album entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            Alias = entity.Alias;
            Enabled = entity.Enabled;
            Public = entity.Public;
            PhotoSessionCaptureCount = entity.PhotoSessionCaptureCount;
            Description = entity.Description;
            ExtraCollageLayouts = entity.CollageLayoutBridges.Select(bridge =>
            {
                var model = new CollageLayoutDisplayModel();

                model.Import(bridge.CollageLayout);

                return model;
            }).ToArray();
            DefaultPictureContentCollageLayout = new CollageLayoutDisplayModel();
            DefaultPhotoSessionContentCollageLayout = new CollageLayoutDisplayModel();

            if (entity.Overlay != null)
            {
                Overlay = new Cloud.Api.Common.Models.ImageMediaDisplayModel();

                Overlay.Import(entity.Overlay);
            }

            DefaultPictureContentCollageLayout.Import(entity.DefaultPictureContentCollageLayout);
            DefaultPhotoSessionContentCollageLayout.Import(entity.DefaultPhotoSessionContentCollageLayout);
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
