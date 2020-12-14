namespace EM.Terminal.Dashboard.Models.Page
{
    public class ManageEventAlbumsPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        public Cloud.Backend.Api.Common.Models.EventDisplayModel Event { get; set; }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            Event = null;
        }
    }
}
