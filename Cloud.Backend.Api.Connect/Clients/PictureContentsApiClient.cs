namespace EM.Cloud.Backend.Api.Connect.Clients
{
    public class PictureContentsApiClient : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Clients.AbstractEntityApiManagementFilteredClient<Persistence.Entities.PictureContent, System.Guid, Common.Models.PictureContentInputModel, Cloud.Api.Common.Models.PictureContentDisplayModel, Common.Models.Filter.PictureContentFilterModel>
    {
        public PictureContentsApiClient(SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider) : base(httpClientHelperProvider)
        {
        }

        protected override string TargetController => "backend/picturecontents";
    }
}