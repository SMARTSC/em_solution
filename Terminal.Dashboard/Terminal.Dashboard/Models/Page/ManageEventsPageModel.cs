namespace EM.Terminal.Dashboard.Models.Page
{
    public class ManageEventsPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        public ManageEventsPageModel()
        {
            Items = new System.Collections.ObjectModel.ObservableCollection<Cloud.Backend.Api.Common.Models.EventDisplayModel>();
        }

        public System.Collections.ObjectModel.ObservableCollection<Cloud.Backend.Api.Common.Models.EventDisplayModel> Items { get; set; }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            Items.Clear();

            Items = null;
        }
    }
}
