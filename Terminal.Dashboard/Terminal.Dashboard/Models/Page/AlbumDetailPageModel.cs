namespace EM.Terminal.Dashboard.Models.Page
{
    public class AlbumDetailPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private Cloud.Backend.Api.Common.Models.AlbumDisplayModel album;

        public Cloud.Backend.Api.Common.Models.AlbumDisplayModel Album { get => album; set => SetProperty(ref album, value); }
    }
}
