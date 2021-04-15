using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.PrintCenter.Pages
{
    public partial class MainPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.MainPageModel>
    {
        private SmartSolucionesCuba.SAPRESSC.Core.Common.Models.PaginationModel<Cloud.Api.Common.Models.ContentDisplayModel> currentContentsPagination;

        public MainPage() : base(new Models.Page.MainPageModel())
        {
            InitializeComponent();

            IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ContentOverviewsCollectionView.SelectedItem = null;

            if (IsEnabled) return;

            LoadFirstContentsPage();

            IsEnabled = true;
        }

        protected override bool OnBackButtonPressed()
        {
            return BindingContextPageModel.IsBusy;
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            currentContentsPagination = null;
        }



        private void FinalizeButton_Clicked(object sender, System.EventArgs e)
        {
            DisplayAlert("Confirmar", "Esta seguro que desea regresar a Inicio?", "Regresar", "Cancelar").ContinueWith(task =>
            {
                if (!task.Result) return;

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    this.NavigationPopModalPageAsync();
                });
            });
        }

        private void ReloadButton_Clicked(object sender, System.EventArgs e)
        {
            LoadFirstContentsPage();
        }

        private void ContentOverviewCollectionViewSwipeGestureRecognizer_LeftSwiped(object sender, Xamarin.Forms.SwipedEventArgs swipedEventArgs)
        {
            if (!BindingContextPageModel.HasNext) return;

            LoadNextContentsPage();
        }

        private void ContentOverviewCollectionViewSwipeGestureRecognizer_RightSwiped(object sender, Xamarin.Forms.SwipedEventArgs swipedEventArgs)
        {
            if (!BindingContextPageModel.HasPrevious) return;

            LoadPreviousContentsPage();
        }

        private void NextButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            LoadNextContentsPage();
        }

        private void PreviousButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            LoadPreviousContentsPage();
        }

        private void ContentOverviewsCollectionView_ChildAdded(object sender, Xamarin.Forms.ElementEventArgs eventArgs)
        {
            var target = eventArgs.Element as Xamarin.Forms.VisualElement;

            target.Scale = 0;

            new SmartSolucionesCuba.XFToolKit.Presentation.Transform.Animations.ScaleToAnimation
            {
                Target = target,
                Duration = "500",
                Scale = 1,
                EasingType = SmartSolucionesCuba.XFToolKit.Presentation.Transform.Enumerations.EasingType.BounceOut
            }.Begin();
        }

        private void LoadFirstContentsPage()
        {
            currentContentsPagination = null;

            LoadNextContentsPage();
        }

        private void LoadNextContentsPage()
        {
            if (BindingContextPageModel.IsBusy) return;

            BindingContextPageModel.IsBusy = true;

            Xamarin.Forms.DependencyService.Get<Providers.ContentPaginationProvider>().GetNextAsync(currentContentsPagination).ContinueWith(task =>
            {
                currentContentsPagination = task.Result;

                Xamarin.Forms.Device.BeginInvokeOnMainThread(PresentCurrentContentsPagination);
            });
        }

        private void LoadPreviousContentsPage()
        {
            if (BindingContextPageModel.IsBusy) return; 
            
            currentContentsPagination = Xamarin.Forms.DependencyService.Get<Providers.ContentPaginationProvider>().GetPrevious(currentContentsPagination);

            PresentCurrentContentsPagination();
        }

        private void PresentCurrentContentsPagination()
        {
            BindingContextPageModel.HasNext = currentContentsPagination.HasNext;
            BindingContextPageModel.HasPrevious = currentContentsPagination.HasPrevious;

            var clearMotionDelay = currentContentsPagination?.List.Count <= 0 ? System.TimeSpan.Zero : System.TimeSpan.FromMilliseconds(500);

            BindingContextPageModel.Items.Clear();

            System.Threading.Tasks.Task.Delay(clearMotionDelay).ContinueWith(_ =>
            {
                BindingContextPageModel.IsBusy = false;

                foreach (var item in currentContentsPagination.List)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        BindingContextPageModel.Items.Add(item);
                    });

                    System.Threading.Thread.Sleep(System.TimeSpan.FromMilliseconds(125));
                }
            });
        }
    }
}
