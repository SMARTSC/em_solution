namespace EM.Cloud.Backend.Api.Connect.Clients
{
    public class EventsApiClient : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Clients.AbstractEntityApiManagementFilteredClient<Persistence.Entities.Event, System.Guid, Common.Models.EventInputModel, Common.Models.EventDisplayModel, Common.Models.Filter.EventFilterModel>
    {
        public EventsApiClient(SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider) : base(httpClientHelperProvider)
        {
        }

        protected override string TargetController => "backend/events";
    }
}