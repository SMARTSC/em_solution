using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.Dashboard.Pages
{
    public partial class ManageEventAlbumsPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.ManageEventAlbumsPageModel>
    {
        public ManageEventAlbumsPage(Cloud.Backend.Api.Common.Models.EventDisplayModel model)
            : base(new Models.Page.ManageEventAlbumsPageModel { Event = model })
        {
            InitializeComponent();

            IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (IsEnabled) return;

            LoadItems();

            IsEnabled = true;
        }

        private void AlbumOverviewGestureRecognizer_Tapped(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPushPageAsync(new AlbumDetailPage((sender as Xamarin.Forms.BindableObject).BindingContext as Cloud.Backend.Api.Common.Models.AlbumDisplayModel, BindingContextPageModel.Event, AlbumsChange_Callback));
        }

        private void AddToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.IsBusy) return;

            this.NavigationPushPageAsync(new AlbumConfigurationPage(BindingContextPageModel.Event, AlbumsChange_Callback));
        }

        private void AlbumsChange_Callback()
        {
            IsEnabled = false;
        }

        private void LoadItems(int page = 0)
        {
            BindingContextPageModel.IsBusy = true;

            System.Threading.Tasks.Task.Run(async () =>
            {
                var client = new Cloud.Backend.Api.Connect.Clients.AlbumsApiClient(Managers.SessionManager.Instance);

                return await client.FilterPaginateAsync(new Cloud.Backend.Api.Common.Models.Filter.AlbumFilterModel { EventId = BindingContextPageModel.Event.Id }, page);
            }).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;

                    if (task.Exception != null || task.IsCanceled)
                    {
                        (Xamarin.Forms.Application.Current as Helpers.Facade.IExceptionHandleHelperFacade).HandleException(task.Exception);
                    }
                    else
                    {
                        if (page == 0)
                        {
                            BindingContextPageModel.Items.Clear();
                        }

                        foreach (var item in task.Result.List)
                        {
                            BindingContextPageModel.Items.Add(item);
                        }
                    }
                });
            });
        }
    }
}