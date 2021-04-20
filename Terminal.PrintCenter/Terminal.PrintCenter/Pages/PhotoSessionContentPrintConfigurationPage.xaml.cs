using EM.Terminal.PrintCenter.Extensions;
using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.PrintCenter.Pages
{
    public partial class PhotoSessionContentPrintConfigurationPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.PhotoSessionContentPrintConfigurationPageModel>, IPhotoSessionContentPrintableViewPresenter
    {
        public PhotoSessionContentPrintConfigurationPage(Cloud.Api.Common.Models.PhotoSessionContentDisplayModel model)
            : base(new Models.Page.PhotoSessionContentPrintConfigurationPageModel
            {
                Content = model,
                PrintLayoutDescriptor = Extend.Layout.Provision.Providers.PrintLayoutDescriptorProvider.Instance.GetPrintLayoutDescriptorById(Xamarin.Forms.DependencyService.Get<Managers.SessionManager>().TargetAlbum.DefaultPhotoSessionContentCollageLayout.Id)
            })
        {
            BindingContextPageModel.Permutation = BindingContextPageModel.PrintLayoutDescriptor.ToDefaultPermutation();

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
            this.NavigationPushModalPageAsync(new PhotoSessionContentPrintPage(BindingContextPageModel.Content, BindingContextPageModel.Permutation, BindingContextPageModel.PrintCopyCount, BindingContextPageModel.PrintLayoutDescriptor));
        }

        #region IPictureContentPrintableViewPresenter implementation

        public Cloud.Api.Common.Models.PhotoSessionContentDisplayModel PhotoSessionContent => BindingContextPageModel.Content;

        public int[] Permutation => BindingContextPageModel.Permutation;

        public System.Action PrintableViewPresentationFailCallbackAction => null;

        public System.Action PrintableViewPresentationReadyCallbackAction => null;

        #endregion
    }
}