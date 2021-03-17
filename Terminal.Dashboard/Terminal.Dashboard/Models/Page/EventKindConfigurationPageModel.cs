namespace EM.Terminal.Dashboard.Models.Page
{
    public class EventKindConfigurationPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private string name;

        public System.Guid? Id { get; set; }

        public string Name { get => name; set => SetProperty(ref name, value); }
    }
}
