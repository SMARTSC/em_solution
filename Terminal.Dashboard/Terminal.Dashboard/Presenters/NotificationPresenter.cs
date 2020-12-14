namespace EM.Terminal.Dashboard.Presenters
{
    public sealed class NotificationPresenter
    {
        private static NotificationPresenter instance;

        public static NotificationPresenter Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NotificationPresenter();
                }

                return instance;
            }
        }

        public void PresentNotification(string title, string message, bool unnattended = true)
        {
            if (unnattended)
            {
                var notificator = Xamarin.Forms.DependencyService.Get<Plugin.Toasts.IToastNotificator>();

                var options = new Plugin.Toasts.NotificationOptions
                {
                    Title = title,
                    Description = message,
                    ClearFromHistory = true
                };

                notificator.Notify(options);
            }
            else
            {
                Xamarin.Forms.Application.Current.MainPage.DisplayAlert(title, message, "Aceptar");
            }
        }
    }
}
