namespace EM.Terminal.Dashboard.Models.Page.Tab
{
    public class ManageEventAlbumPhotoSessionContentsTabPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        public ManageEventAlbumPhotoSessionContentsTabPageModel()
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
