using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.PrintCenter.Pages
{
    public partial class StartUpPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.StartUpPageModel>
    {
        private SmartSolucionesCuba.XFToolKit.Presentation.Transform.Animations.HeartAnimation screenInteractionSuggestLabelAnimation;

        public StartUpPage() : base(new Models.Page.StartUpPageModel { IsBusy = true })
        {
            InitializeComponent();

            IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (IsEnabled) return;

            var sessionManager = Xamarin.Forms.DependencyService.Get<Managers.SessionManager>();

            if (!sessionManager.HasAuthenticatedSession)
            {
                this.NavigationPushModalPageAsync(new SessionConfigurationPage());

                return;
            }

            BindingContextPageModel.TargetEvent = sessionManager.TargetEvent;
            BindingContextPageModel.TargetAlbum = sessionManager.TargetAlbum;
            BindingContextPageModel.IsBusy = false;

            IsEnabled = true;

            PerformAppearingAnimation();
        }

        private void InteractiveLayerBoxViewTapGestureRecognizer_Tapped(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPushModalPageAsync(new MainPage());
        }

        private void ScreenInteractionSuggestLabelAnimation_Completed(object sender, System.EventArgs eventArgs)
        {
            System.Threading.Tasks.Task.Delay(System.TimeSpan.FromSeconds(3)).ContinueWith(_ =>
            {
                screenInteractionSuggestLabelAnimation?.Begin();
            });
        }

        private void PerformAppearingAnimation()
        {
            if (screenInteractionSuggestLabelAnimation != null) return;

            screenInteractionSuggestLabelAnimation = new SmartSolucionesCuba.XFToolKit.Presentation.Transform.Animations.HeartAnimation
            {
                Target = ScreenInteractionSuggestLabel
            };

            screenInteractionSuggestLabelAnimation.Completed += ScreenInteractionSuggestLabelAnimation_Completed;

            screenInteractionSuggestLabelAnimation.Begin();
        }
    }
}