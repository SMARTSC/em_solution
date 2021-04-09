namespace EM.Terminal.PhotoMirror.Wind0ws
{
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Presenters.WorkflowStatePresenter.Instance.BeginPresentation(Content as System.Windows.Controls.Panel);
        }
    }
}
