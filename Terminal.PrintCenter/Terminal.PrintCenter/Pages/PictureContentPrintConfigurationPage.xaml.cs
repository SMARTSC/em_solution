using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.PrintCenter.Pages
{
    public partial class PictureContentPrintConfigurationPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.PictureContentPrintConfigurationPageModel>, IPictureContentPrintableViewPresenter
    {
        public PictureContentPrintConfigurationPage(Cloud.Api.Common.Models.PictureContentDisplayModel model)
            : base(new Models.Page.PictureContentPrintConfigurationPageModel 
            { 
                Content = model, 
                PrintLayoutDescriptor = Extend.Layout.Provision.Providers.PrintLayoutDescriptorProvider.Instance.GetPrintLayoutDescriptorById(Xamarin.Forms.DependencyService.Get<Managers.SessionManager>().TargetAlbum.DefaultPictureContentCollageLayout.Id) 
            })
        {
            InitializeComponent();
        }

        private void CloseButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPopModalPageAsync();
        }

        private void LessPrintCopyCountButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.PrintCopyCount <= 1) return;

            BindingContextPageModel.PrintCopyCount--;
        }

        private void MorePrintCopyCountButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            BindingContextPageModel.PrintCopyCount++;
        }

        private void RequestPrintButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            DisplayAlert("Confirmar", "Está seguro que desea imprimir esta fotografía?", "Confirmar", "Cancelar").ContinueWith(task =>
            {
                if (!task.Result) return;

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    this.NavigationPushModalPageAsync(new PictureContentPrintPage(BindingContextPageModel.Content, BindingContextPageModel.PrintCopyCount, BindingContextPageModel.PrintLayoutDescriptor));
                });
            });
        }

        #region IPictureContentPrintableViewPresenter implementation

        public Cloud.Api.Common.Models.PictureContentDisplayModel PictureContent => BindingContextPageModel.Content;

        public System.Action PrintableViewPresentationFailCallbackAction => null;

        public System.Action PrintableViewPresentationReadyCallbackAction => null;

        #endregion
    }
}