namespace EM.Terminal.PhotoMirror.Wind0ws
{
    public partial class ConfigurationOptionsWindow : System.Windows.Window
    {
        public ConfigurationOptionsWindow()
        {
            InitializeComponent();
        }

        private void CloseAppButton_Click(object sender, System.Windows.RoutedEventArgs eventArgs)
        {
            var result = System.Windows.MessageBox.Show("Está seguro que desea cerrar la aplicación?", "Confirmar", System.Windows.MessageBoxButton.YesNo);

            if (result == System.Windows.MessageBoxResult.No) return;

            System.Windows.Application.Current.Shutdown();
        }
    }
}
