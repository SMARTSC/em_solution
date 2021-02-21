namespace EM.Terminal.Dashboard.Models.Page
{
    public class ManageEventAlbumPhotoSessionContentsPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        public ManageEventAlbumPhotoSessionContentsPageModel()
        {
            Items = new System.Collections.ObjectModel.ObservableCollection<Cloud.Api.Common.Models.PhotoSessionContentDisplayModel>();
        }

        public System.Collections.ObjectModel.ObservableCollection<Cloud.Api.Common.Models.PhotoSessionContentDisplayModel> Items { get; set; }

        public Cloud.Backend.Api.Common.Models.AlbumDisplayModel Album { get; set; }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            Items.Clear();

            Items = null;
        }
    }
}
