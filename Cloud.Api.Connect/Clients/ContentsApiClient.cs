namespace EM.Cloud.Api.Connect.Clients
{
    public class ContentsApiClient : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Clients.AbstractEntityApiManagementFilteredClient<Persistence.Entities.Content, System.Guid, Common.Models.ContentInputModel, Common.Models.ContentDisplayModel, Common.Models.Filter.ContentFilterModel>
    {
        public ContentsApiClient(SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider) : base(httpClientHelperProvider)
        {
        }

        protected override string TargetController => "contents";
    }
}
