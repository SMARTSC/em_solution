using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.Dashboard.Pages
{
    public partial class EventKindConfigurationPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.EventKindConfigurationPageModel>
    {
        private bool pageModelHasChanges;

        private System.Action successfulCompleteCallbackAction;

        public EventKindConfigurationPage(System.Action successfulCompleteCallbackAction)
            : this(null, successfulCompleteCallbackAction)
        {
        }

        public EventKindConfigurationPage(Cloud.Backend.Api.Common.Models.EventKindDisplayModel model, System.Action successfulCompleteCallbackAction)
            : base(Factories.Model.Page.EventKindConfigurationPageModelFactory.Build(model))
        {
            this.successfulCompleteCallbackAction = successfulCompleteCallbackAction;

            InitializeComponent();

            BindingContextPageModel.PropertyChanged += PageModel_PropertyChanged;

            if (model == null)
            {
                Title = "Nuevo Tipo de Evento";

                return;
            }

            Title = "Modificar Tipo de Evento";
        }

        protected override bool OnBackButtonPressed()
        {
            CancelToolbarItem_Clicked(this, System.EventArgs.Empty);

            return true;
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            BindingContextPageModel.PropertyChanged -= PageModel_PropertyChanged;

            successfulCompleteCallbackAction = null;
        }

        private void PageModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            pageModelHasChanges = true;
        }

        private void ApplyToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (!pageModelHasChanges)
            {
                this.NavigationPopPageAsync();

                return;
            }

            BindingContextPageModel.IsBusy = true;

            System.Threading.Tasks.Task.Run(async () =>
            {
                var client = new Cloud.Backend.Api.Connect.Clients.EventKindsApiClient(Managers.SessionManager.Instance);

                var model = new Cloud.Backend.Api.Common.Models.EventKindInputModel
                {
                    Name = BindingContextPageModel.Name
                };

                if (BindingContextPageModel.Id != null)
                {
                    model.Id = BindingContextPageModel.Id.Value;

                    await client.EditAsync(model);
                }
                else
                {
                    await client.CreateAsync(model);
                }
            }).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Exception == null && !task.IsCanceled)
                    {
                        Presenters.NotificationPresenter.Instance.PresentNotification("Información", "Cambios aplicados con éxito.");

                        successfulCompleteCallbackAction?.Invoke();

                        this.NavigationPopPageAsync();
                    }
                    else
                    {
                        BindingContextPageModel.IsBusy = false;

                        (Xamarin.Forms.Application.Current as Helpers.Facade.IExceptionHandleHelperFacade).HandleException(task.Exception);
                    }
                });
            });
        }

        private void CancelToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (!pageModelHasChanges)
            {
                this.NavigationPopPageAsync();

                return;
            }

            DisplayAlert("Confirmar", "Está seguro que desea regresar sin guardar los cambios?", "Regresar", "Cancelar").ContinueWith(task =>
            {
                if (!task.Result) return;

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    this.NavigationPopPageAsync();
                });
            });
        }
    }
}