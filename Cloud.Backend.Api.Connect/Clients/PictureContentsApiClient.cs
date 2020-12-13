namespace EM.Cloud.Backend.Api.Connect.Clients
{
    public class PictureContentsApiClient : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Clients.AbstractEntityApiManagementClient<Persistence.Entities.PictureContent, System.Guid, Common.Models.PictureContentInputModel, Cloud.Api.Common.Models.PictureContentDisplayModel>
    {
        public PictureContentsApiClient(SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider) : base(httpClientHelperProvider)
        {
        }

        protected override string TargetController => "backend/picturecontents";
    }
}
