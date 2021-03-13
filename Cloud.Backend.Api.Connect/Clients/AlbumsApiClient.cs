namespace EM.Cloud.Backend.Api.Connect.Clients
{
    public class AlbumsApiClient : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Clients.AbstractEntityApiManagementFilteredClient<Persistence.Entities.Album, System.Guid, Common.Models.AlbumInputModel, Common.Models.AlbumDisplayModel, Common.Models.Filter.AlbumFilterModel>
    {
        public AlbumsApiClient(SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider) : base(httpClientHelperProvider)
        {
        }

        protected override string TargetController => "backend/albums";
    }
}
