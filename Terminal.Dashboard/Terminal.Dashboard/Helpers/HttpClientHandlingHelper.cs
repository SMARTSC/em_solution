namespace EM.Terminal.Dashboard.Helpers
{
    public sealed class HttpClientHandlingHelper : System.Net.Http.HttpClientHandler
    {
        protected override System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> SendAsync(System.Net.Http.HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(Managers.SessionManager.Instance.AuthenticationType, Managers.SessionManager.Instance.AuthorizationToken);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
