namespace EM.Cloud.Backend.Api.Connect.Clients
{
    public class PhotoSessionContentsApiClient : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Clients.AbstractEntityApiManagementFilteredClient<Persistence.Entities.PhotoSessionContent, System.Guid, Common.Models.PhotoSessionContentInputModel, Cloud.Api.Common.Models.PhotoSessionContentDisplayModel, Common.Models.Filter.PhotoSessionContentFilterModel>
    {
        public PhotoSessionContentsApiClient(SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider) : base(httpClientHelperProvider)
        {
        }

        protected override string TargetController => "backend/photosessioncontents";

        public virtual async System.Threading.Tasks.Task AgregateAsync(System.Guid key, Common.Models.ImageMediaInputModel inputModel)
        {
            await httpClientHelperProvider.HttpClientHelper.PutAsync(string.Format("{0}/{1}", TargetController, key), inputModel);
        }
    }
}
