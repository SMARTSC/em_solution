namespace EM.Terminal.Dashboard.Models.Page.Tab
{
    public class ManageEventAlbumPictureContentsTabPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        public ManageEventAlbumPictureContentsTabPageModel()
        {
            Items = new System.Collections.ObjectModel.ObservableCollection<Cloud.Api.Common.Models.PictureContentDisplayModel>();
        }

        public System.Collections.ObjectModel.ObservableCollection<Cloud.Api.Common.Models.PictureContentDisplayModel> Items { get; set; }

        public Cloud.Backend.Api.Common.Models.AlbumDisplayModel Album { get; set; }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            Items.Clear();

            Items = null;
        }
    }
}
