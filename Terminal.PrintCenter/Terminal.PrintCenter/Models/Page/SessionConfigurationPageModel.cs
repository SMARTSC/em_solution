namespace EM.Terminal.PrintCenter.Models.Page
{
    public class SessionConfigurationPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private Cloud.Backend.Api.Common.Models.EventDisplayModel targetEvent;
        private System.Collections.ObjectModel.ObservableCollection<Cloud.Backend.Api.Common.Models.EventDisplayModel> activeEvents;

        public Cloud.Backend.Api.Common.Models.EventDisplayModel TargetEvent { get => targetEvent; set => SetProperty(ref targetEvent, value); }

        public System.Collections.ObjectModel.ObservableCollection<Cloud.Backend.Api.Common.Models.EventDisplayModel> ActiveEvents { get => activeEvents; set => SetProperty(ref activeEvents, value); }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            ActiveEvents.Clear();
            ActiveEvents = null;
            TargetEvent = null;
        }
    }
}