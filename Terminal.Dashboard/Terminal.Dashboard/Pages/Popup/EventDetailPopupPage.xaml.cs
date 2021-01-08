namespace EM.Terminal.Dashboard.Pages.Popup
{
    public partial class EventDetailPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public EventDetailPopupPage(Cloud.Backend.Api.Common.Models.EventDisplayModel model)
        {
            InitializeComponent();

            BindingContext = model;
        }
    }
}