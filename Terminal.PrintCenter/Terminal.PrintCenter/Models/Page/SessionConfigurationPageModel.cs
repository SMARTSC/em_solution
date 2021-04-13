namespace EM.Terminal.PrintCenter.Models.Page
{
    public class SessionConfigurationPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private Cloud.Backend.Api.Common.Models.EventDisplayModel @event;

        public SessionConfigurationPageModel()
        {
            AvailableEvents = new System.Collections.ObjectModel.ObservableCollection<Cloud.Backend.Api.Common.Models.EventDisplayModel>();
        }

        public Cloud.Backend.Api.Common.Models.EventDisplayModel Event { get => @event; set => SetProperty(ref @event, value); }

        public System.Collections.ObjectModel.ObservableCollection<Cloud.Backend.Api.Common.Models.EventDisplayModel> AvailableEvents { get; set; }
    }
}
