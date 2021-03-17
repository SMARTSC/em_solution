namespace EM.Terminal.Dashboard.Models.Page
{
    public class ManageEventsPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private bool isLoading;

        public ManageEventsPageModel()
        {
            Items = new System.Collections.ObjectModel.ObservableCollection<Cloud.Backend.Api.Common.Models.EventDisplayModel>();
        }

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        public System.Collections.ObjectModel.ObservableCollection<Cloud.Backend.Api.Common.Models.EventDisplayModel> Items { get; set; }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            Items.Clear();

            Items = null;
        }
    }
}
