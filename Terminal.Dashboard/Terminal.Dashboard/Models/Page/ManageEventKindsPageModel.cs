namespace EM.Terminal.Dashboard.Models.Page
{
    public class ManageEventKindsPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        public ManageEventKindsPageModel()
        {
            Items = new System.Collections.ObjectModel.ObservableCollection<Cloud.Backend.Api.Common.Models.EventKindDisplayModel>();
        }

        public System.Collections.ObjectModel.ObservableCollection<Cloud.Backend.Api.Common.Models.EventKindDisplayModel> Items { get; set; }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            Items.Clear();

            Items = null;
        }
    }
}
