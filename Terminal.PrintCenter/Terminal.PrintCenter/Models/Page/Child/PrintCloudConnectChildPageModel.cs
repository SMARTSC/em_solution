namespace EM.Terminal.PrintCenter.Models.Page.Child
{
    public class PrintCloudConnectChildPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private string host;
        private int? port;
        private bool isSecure;

        public string Host { get => host; set => SetProperty(ref host, value); }

        public int? Port { get => port; set => SetProperty(ref port, value); }

        public bool IsSecure { get => isSecure; set => SetProperty(ref isSecure, value); }
    }
}
