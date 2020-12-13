using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.Dashboard.Pages
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [System.ComponentModel.DesignTimeVisible(false)]
    public partial class MainPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.MainPageModel>
    {
        public MainPage() : base(new Models.Page.MainPageModel { IsBusy = true })
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!Managers.SessionManager.Instance.HasAuthenticatedIdentity)
            {
                this.NavigationPushModalPageAsync(new UserAuthenticationPage());
            }
            else
            {
                ReloadActions();

                BindingContextPageModel.IsBusy = false;

                IsEnabled = true;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            DisplayAlert("Aviso", "Está seguro que desea salir de la aplicación?", "Salir", "Cancelar").ContinueWith(task =>
            {
                if (task.Result)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        Xamarin.Forms.DependencyService.Get<Managers.Facade.IApplicationManagerFacade>().Quit();
                    });
                }
            });

            return true;
        }

        private void SignOutToolbarItem_Activated(object sender, System.EventArgs eventArgs)
        {
            DisplayAlert("Confirmar", "Esta seguro que desea terminar la sesión?", "Cerrar", "Cancelar").ContinueWith(task =>
            {
                if (!task.Result) return;

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    Managers.SessionManager.Instance.SignOut();

                    IsEnabled = false;

                    this.NavigationPushModalPageAsync(new UserAuthenticationPage());
                });
            });
        }

        private void ActionFrame_Tapped(object sender, System.EventArgs eventArgs)
        {
            //switch (((sender as Xamarin.Forms.BindableObject).BindingContext as Models.View.ActionViewModel).Kind)
            //{
            //    case Enumerations.ActionKind.ManageImages:
            //        this.NavigationPushPageAsync(new ManageImagesPage());

            //        break;

            //    case Enumerations.ActionKind.ManageVideos:
            //        this.NavigationPushPageAsync(new ManageVideosPage());

            //        break;

            //    case Enumerations.ActionKind.ManagePlayers:
            //        this.NavigationPushPageAsync(new ManagePlayersPage());

            //        break;

            //    case Enumerations.ActionKind.ManageScreenplays:
            //        this.NavigationPushPageAsync(new ManageProgrammingPage());

            //        break;

            //    case Enumerations.ActionKind.ConfigAccount:
            //        this.NavigationPushPageAsync(new AccountIdentityConfigurationPage());

            //        break;

            //    case Enumerations.ActionKind.ManagePlugins:
            //        this.NavigationPushPageAsync(new ManageExtensionsPage());

            //        break;

            //    default:
            //        DisplayAlert("Aviso", "Sección no disponible para esta versión", "Aceptar");

            //        break;
            //}
        }

        private void ReloadActions()
        {
            BindingContextPageModel.ActionGroups.Clear();

            BindingContextPageModel.ActionGroups.Add(new Models.View.ActionGroupViewModel("Seguridad", new Models.View.ActionViewModel[]
            {
                new Models.View.ActionViewModel
                {
                    Kind = Enumerations.ActionKind.ManageUsers,
                    Caption = "Usuarios",
                    IconGlyph = "\uf0d6",
                    IsEnabled = false
                },
                new Models.View.ActionViewModel
                {
                    Kind = Enumerations.ActionKind.ManageUserGroups,
                    Caption = "Grupos de Usuarios",
                    IconGlyph = "\uF431",
                    IsEnabled = false
                }
            }));

            BindingContextPageModel.ActionGroups.Add(new Models.View.ActionGroupViewModel("Negocio", new Models.View.ActionViewModel[]
            {
                new Models.View.ActionViewModel
                {
                    Kind = Enumerations.ActionKind.ManageEvents,
                    Caption = "Eventos",
                    IconGlyph = "\uFE3A",
                    IsEnabled = true
                }
            }));
        }
    }
}
