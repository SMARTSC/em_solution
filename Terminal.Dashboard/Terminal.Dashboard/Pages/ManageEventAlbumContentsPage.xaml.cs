namespace EM.Terminal.Dashboard.Pages
{
    public partial class ManageEventAlbumContentsPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.TabbedPage
    {
        public ManageEventAlbumContentsPage(Cloud.Backend.Api.Common.Models.AlbumDisplayModel model)
        {
            InitializeComponent();

            Children.Add(new Tab.ManageEventAlbumPictureContentsTabPage(model));
            Children.Add(new Tab.ManageEventAlbumPhotoSessionContentsTabPage(model));
        }
    }
}