namespace EM.Terminal.Dashboard.Models.Page
{
    public class MainPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        public MainPageModel()
        {
            ActionGroups = new System.Collections.ObjectModel.ObservableCollection<View.ActionGroupViewModel>();
        }

        public System.Collections.ObjectModel.ObservableCollection<View.ActionGroupViewModel> ActionGroups { get; set; }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            foreach (var item in ActionGroups)
            {
                item.Clear();
            }

            ActionGroups.Clear();

            ActionGroups = null;
        }
    }
}
