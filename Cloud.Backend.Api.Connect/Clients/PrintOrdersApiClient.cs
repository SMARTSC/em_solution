namespace EM.Cloud.Backend.Api.Connect.Clients
{
    public class PrintOrdersApiClient : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Clients.AbstractEntityApiManagementFilteredClient<Persistence.Entities.PrintOrder, long, Common.Models.PrintOrderInputModel, Common.Models.PrintOrderDisplayModel, Common.Models.Filter.PrintOrderFilterModel>
    {
        public PrintOrdersApiClient(SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider) : base(httpClientHelperProvider)
        {
        }

        protected override string TargetController => "backend/printorders";

        public async System.Threading.Tasks.Task<bool> StackEnabled()
        {
            return await httpClientHelperProvider.HttpClientHelper.GetAsync<bool>(string.Format("{0}/stackenabled", TargetController));
        }
    }
}
