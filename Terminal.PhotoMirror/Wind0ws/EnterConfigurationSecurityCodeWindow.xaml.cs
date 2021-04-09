namespace EM.Terminal.PhotoMirror.Wind0ws
{
    public partial class EnterConfigurationSecurityCodeWindow : System.Windows.Window
    {
        private readonly bool isForConfiguration;

        public EnterConfigurationSecurityCodeWindow(bool isForConfiguration = false)
        {
            this.isForConfiguration = isForConfiguration;

            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            if (!isForConfiguration) return;

            if (Managers.SessionManager.Instance.ConfigurationSecurityCode != null) return;

            var result = System.Windows.MessageBox.Show("Está seguro que desea cancelar y no usar un Código de Seguridad para restringir el acceso a la Configuración?", "Confirmar", System.Windows.MessageBoxButton.YesNo);

            eventArgs.Cancel = result == System.Windows.MessageBoxResult.No;
        }

        private void ApplyButton_Click(object sender, System.Windows.RoutedEventArgs eventArgs)
        {
            if (string.IsNullOrEmpty(LastCharactarCodeTextBox.Text))
            {
                System.Windows.MessageBox.Show("Debe completar el Código de Seguridad para la Configuración para continuar.", "Aviso");

                return;
            }

            var securityCode = new char[] { FirstCharactarCodeTextBox.Text[0], SecoundCharactarCodeTextBox.Text[0], ThirthCharactarCodeTextBox.Text[0], LastCharactarCodeTextBox.Text[0] };

            if (!isForConfiguration)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Managers.SessionManager.Instance.ConfigurationSecurityCode[i] != securityCode[i])
                    {
                        System.Windows.MessageBox.Show(string.Format("Código incorrecto!.{0}{0}Debe proporcionar el correcto para acceder a la Configuración.", System.Environment.NewLine), "Aviso");

                        return;
                    }
                }
            }
            else
            {
                Managers.SessionManager.Instance.ConfigurationSecurityCode = securityCode;
            }

            DialogResult = true;

            Close();
        }

        private void CharacterButton_Clicked(object sender, System.Windows.RoutedEventArgs eventArgs)
        {
            if (string.IsNullOrEmpty(FirstCharactarCodeTextBox.Text))
            {
                FirstCharactarCodeTextBox.Text = (sender as System.Windows.Controls.Button).Content.ToString();

                return;
            }

            if (string.IsNullOrEmpty(SecoundCharactarCodeTextBox.Text))
            {
                SecoundCharactarCodeTextBox.Text = (sender as System.Windows.Controls.Button).Content.ToString();

                return;
            }

            if (string.IsNullOrEmpty(ThirthCharactarCodeTextBox.Text))
            {
                ThirthCharactarCodeTextBox.Text = (sender as System.Windows.Controls.Button).Content.ToString();

                return;
            }

            if (string.IsNullOrEmpty(LastCharactarCodeTextBox.Text))
            {
                LastCharactarCodeTextBox.Text = (sender as System.Windows.Controls.Button).Content.ToString();

                return;
            }
        }

        private void ClearButton_Clicked(object sender, System.Windows.RoutedEventArgs eventArgs)
        {
            if (!string.IsNullOrEmpty(LastCharactarCodeTextBox.Text))
            {
                LastCharactarCodeTextBox.Text = null;

                return;
            }

            if (!string.IsNullOrEmpty(ThirthCharactarCodeTextBox.Text))
            {
                ThirthCharactarCodeTextBox.Text = null;

                return;
            }

            if (!string.IsNullOrEmpty(SecoundCharactarCodeTextBox.Text))
            {
                SecoundCharactarCodeTextBox.Text = null;

                return;
            }

            if (!string.IsNullOrEmpty(FirstCharactarCodeTextBox.Text))
            {
                FirstCharactarCodeTextBox.Text = null;
            }
        }
    }
}
