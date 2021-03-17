namespace EM.Cloud.Backend.Api.Connect.Clients
{
    public class EventKindsApiClient : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Clients.AbstractEntityApiManagementClient<Persistence.Entities.EventKind, System.Guid, Common.Models.EventKindInputModel, Common.Models.EventKindDisplayModel>
    {
        public EventKindsApiClient(SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider) : base(httpClientHelperProvider)
        {
        }

        protected override string TargetController => "backend/eventkinds";
    }
}
