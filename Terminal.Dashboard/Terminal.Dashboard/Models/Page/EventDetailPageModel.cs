namespace EM.Terminal.Dashboard.Models.Page
{
    public class EventDetailPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private Cloud.Backend.Api.Common.Models.EventDisplayModel @event;

        public Cloud.Backend.Api.Common.Models.EventDisplayModel Event { get => @event; set => SetProperty(ref @event, value); }
    }
}
