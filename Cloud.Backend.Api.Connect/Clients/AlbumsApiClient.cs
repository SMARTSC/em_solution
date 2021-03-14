namespace EM.Cloud.Backend.Api.Connect.Clients
{
    public class AlbumsApiClient : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Clients.AbstractEntityApiManagementFilteredClient<Persistence.Entities.Album, System.Guid, Common.Models.AlbumInputModel, Common.Models.AlbumDisplayModel, Common.Models.Filter.AlbumFilterModel>
    {
        public AlbumsApiClient(SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider) : base(httpClientHelperProvider)
        {
        }

        protected override string TargetController => "backend/albums";

        public System.Threading.Tasks.Task<Common.Models.AlbumLockDisplayModel[]> GetLocksAsync(System.Guid albumId)
        {
            return httpClientHelperProvider.HttpClientHelper.GetAsync<Common.Models.AlbumLockDisplayModel[]>(string.Format("{0}/{1}/locks", TargetController, albumId));
        }

        public System.Threading.Tasks.Task CreateLockAsync(Common.Models.AlbumLockInputModel inputModel)
        {
            return httpClientHelperProvider.HttpClientHelper.PostAsync(string.Format("{0}/lock", TargetController), inputModel);
        }

        public System.Threading.Tasks.Task EditLockAsync(Common.Models.AlbumLockInputModel inputModel)
        {
            return httpClientHelperProvider.HttpClientHelper.PutAsync(string.Format("{0}/lock", TargetController), inputModel);
        }

        public System.Threading.Tasks.Task DeleteLockAsync(System.Guid albumId, System.Guid lockId)
        {
            return httpClientHelperProvider.HttpClientHelper.DeleteAsync(string.Format("{0}/{1}/locks/{2}", TargetController, albumId, lockId));
        }
    }
}
