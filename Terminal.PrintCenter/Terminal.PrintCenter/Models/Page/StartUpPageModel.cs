namespace EM.Terminal.PrintCenter.Models.Page
{
    public class StartUpPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private Cloud.Backend.Api.Common.Models.EventDisplayModel targetEvent;
        private Cloud.Backend.Api.Common.Models.AlbumDisplayModel targetAlbum;

        public Cloud.Backend.Api.Common.Models.EventDisplayModel TargetEvent { get => targetEvent; set => SetProperty(ref targetEvent, value); }

        public Cloud.Backend.Api.Common.Models.AlbumDisplayModel TargetAlbum { get => targetAlbum; set => SetProperty(ref targetAlbum, value); }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            TargetAlbum = null;
            TargetEvent = null;
        }
    }
}
