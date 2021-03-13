namespace EM.Terminal.Dashboard.Models.Page
{
    public class ManageEventAlbumsPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        public ManageEventAlbumsPageModel()
        {
            Items = new System.Collections.ObjectModel.ObservableCollection<Cloud.Backend.Api.Common.Models.AlbumDisplayModel>();
        }

        public Cloud.Backend.Api.Common.Models.EventDisplayModel Event { get; set; }

        public System.Collections.ObjectModel.ObservableCollection<Cloud.Backend.Api.Common.Models.AlbumDisplayModel> Items { get; set; }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            Items.Clear();

            Items = null;

            Event = null;
        }
    }
}
