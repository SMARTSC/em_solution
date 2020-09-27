namespace EM.Cloud.Models.View
{
    public class AlbumDisplayViewModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Album, System.Guid>
    {
        public AlbumDisplayViewModel()
        {
            ExtraCollageLayouts = new System.Collections.Generic.List<CollageLayoutDisplayViewModel>();
        }

        public System.Guid Id { get; set; }

        public EventDisplayViewModel Event { get; set; }

        public string Title { get; set; }

        public string Alias { get; set; }

        public bool Enabled { get; set; }

        public bool Public { get; set; }

        public int PhotoSessionCaptureCount { get; set; }

        public CollageLayoutDisplayViewModel DefaultPictureContentCollageLayout { get; set; }

        public CollageLayoutDisplayViewModel DefaultPhotoSessionContentCollageLayout { get; set; }

        public string Description { get; set; }

        public System.Collections.Generic.ICollection<CollageLayoutDisplayViewModel> ExtraCollageLayouts { get; set; }

        public void Import(Persistence.Entities.Album entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            Alias = entity.Alias;
            Enabled = entity.Enabled;
            Public = entity.Public;
            Description = entity.Description;
            PhotoSessionCaptureCount = entity.PhotoSessionCaptureCount;
            DefaultPictureContentCollageLayout = new CollageLayoutDisplayViewModel();
            DefaultPhotoSessionContentCollageLayout = new CollageLayoutDisplayViewModel();

            DefaultPictureContentCollageLayout.Import(entity.DefaultPictureContentCollageLayout);
            DefaultPhotoSessionContentCollageLayout.Import(entity.DefaultPhotoSessionContentCollageLayout);

            foreach (var bridge in entity.CollageLayoutBridges)
            {
                var model = new CollageLayoutDisplayViewModel();

                model.Import(bridge.CollageLayout);

                ExtraCollageLayouts.Add(model);
            }
        }
    }
}
