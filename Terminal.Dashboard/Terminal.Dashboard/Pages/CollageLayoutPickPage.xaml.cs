using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.Dashboard.Pages
{
    public partial class CollageLayoutPickPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.ContentPage
    {
        private System.Action<Extend.Layout.Descriptors.Facade.ICollageLayoutDescriptorFacade> pickedCallbackAction;

        public CollageLayoutPickPage(System.Action<Extend.Layout.Descriptors.Facade.ICollageLayoutDescriptorFacade> pickedCallbackAction)
        {
            this.pickedCallbackAction = pickedCallbackAction;

            InitializeComponent();

            BindingContext = Extend.Layout.Provision.Providers.CollageLayoutDescriptorProvider.Instance.GetCollageLayoutDescriptors();
        }

        private void PrintLayoutGestureRecognizer_Tapped(object sender, System.EventArgs eventArgs)
        {
            DisplayAlert("Confirmar", "Está seguro que desea seleccionar esta Platilla de Impresión?", "Confirmar", "Cancelar").ContinueWith(task =>
            {
                if (!task.Result) return;

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    this.NavigationPopPageAsync();

                    pickedCallbackAction?.Invoke((sender as Xamarin.Forms.BindableObject).BindingContext as Extend.Layout.Descriptors.Facade.ICollageLayoutDescriptorFacade);
                });
            });
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            pickedCallbackAction = null;
        }
    }
}