namespace EM.Terminal.PhotoMirror.Uploader
{
    [System.ComponentModel.RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        private readonly System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
        private readonly System.ServiceProcess.ServiceInstaller serviceInstaller;

        public Installer()
        {
            InitializeComponent();

            serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller
            {
                Account = System.ServiceProcess.ServiceAccount.LocalSystem,
                Password = null,
                Username = null
            };

            serviceInstaller = new System.ServiceProcess.ServiceInstaller
            {
                Description = Common.Providers.CommonConstantProvider.UPLOAD_SERVICE_DESCRIPTION,
                DisplayName = Common.Providers.CommonConstantProvider.UPLOAD_SERVICE_DISPLAY_NAME,
                ServiceName = Common.Providers.CommonConstantProvider.UPLOAD_SERVICE_NAME,
                StartType = System.ServiceProcess.ServiceStartMode.Manual
            };

            Installers.AddRange(new System.Configuration.Install.Installer[] {
                serviceProcessInstaller,
                serviceInstaller
            });
        }
    }
}
