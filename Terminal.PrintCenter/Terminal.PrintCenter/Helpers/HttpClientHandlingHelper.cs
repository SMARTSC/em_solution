namespace EM.Terminal.PrintCenter.Helpers
{
    public sealed class HttpClientHandlingHelper : System.Net.Http.HttpClientHandler
    {
        protected override System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> SendAsync(System.Net.Http.HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var sessionManager = Xamarin.Forms.DependencyService.Get<Managers.SessionManager>();

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(sessionManager.AuthenticationType, sessionManager.AuthorizationToken);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
