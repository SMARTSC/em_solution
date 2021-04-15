using System.Linq;

namespace EM.Terminal.PrintCenter.Providers
{
    public sealed class ContentPaginationProvider
    {
        private System.DateTime paginationStartAt;

        private readonly System.Collections.Generic.List<SmartSolucionesCuba.SAPRESSC.Core.Common.Models.PaginationModel<Cloud.Api.Common.Models.ContentDisplayModel>> paginations;

        public ContentPaginationProvider()
        {
            paginations = new System.Collections.Generic.List<SmartSolucionesCuba.SAPRESSC.Core.Common.Models.PaginationModel<Cloud.Api.Common.Models.ContentDisplayModel>>();
        }

        public async System.Threading.Tasks.Task<SmartSolucionesCuba.SAPRESSC.Core.Common.Models.PaginationModel<Cloud.Api.Common.Models.ContentDisplayModel>> GetNextAsync(SmartSolucionesCuba.SAPRESSC.Core.Common.Models.PaginationModel<Cloud.Api.Common.Models.ContentDisplayModel> current)
        {
            SmartSolucionesCuba.SAPRESSC.Core.Common.Models.PaginationModel<Cloud.Api.Common.Models.ContentDisplayModel> result;

            var contentApiConnectClient = new Cloud.Api.Connect.Clients.ContentsApiClient(Xamarin.Forms.DependencyService.Get<HttpClientHelperProvider>());

            if (current == null || current.Page >= paginations.Count)
            {
                if (current == null) paginationStartAt = System.DateTime.Now;

                var filter = new Cloud.Api.Common.Models.Filter.ContentFilterModel
                {
                    BeforeAt = paginationStartAt,
                    TargetKinds = new System.Collections.Generic.Dictionary<Cloud.Api.Common.Enumerations.ContentKind, bool>
                    {
                        [Cloud.Api.Common.Enumerations.ContentKind.Picture] = true,
                        [Cloud.Api.Common.Enumerations.ContentKind.Session] = true,
                        [Cloud.Api.Common.Enumerations.ContentKind.Animate] = false,
                        [Cloud.Api.Common.Enumerations.ContentKind.Clip] = false
                    },
                    AlbumId = Xamarin.Forms.DependencyService.Get<Managers.SessionManager>().TargetAlbum.Id
                };

                result = await contentApiConnectClient.FilterPaginateAsync(filter, current == null ? 1 : current.Page + 1, 3 * Helpers.ResponsivePresentationHelper.ContentOverviewCollectionViewSnap);

                var paginationList = result.List;

                result.List = new System.Collections.Generic.List<Cloud.Api.Common.Models.ContentDisplayModel>();

                var pictureContentsApiClient = new Cloud.Backend.Api.Connect.Clients.PictureContentsApiClient(Xamarin.Forms.DependencyService.Get<HttpClientHelperProvider>());
                var photoSessionContentsApiClient = new Cloud.Backend.Api.Connect.Clients.PhotoSessionContentsApiClient(Xamarin.Forms.DependencyService.Get<HttpClientHelperProvider>());

                var iterationCount = paginationList.Count();

                for (int i = 0; i < iterationCount; i++)
                {
                    var targetContent = paginationList.ElementAt(i);

                    if (targetContent.Kind == Cloud.Api.Common.Enumerations.ContentKind.Picture)
                    {
                        result.List.Add(await pictureContentsApiClient.GetAsync(targetContent.Id));
                    }
                    else if (targetContent.Kind == Cloud.Api.Common.Enumerations.ContentKind.Session)
                    {
                        result.List.Add(await photoSessionContentsApiClient.GetAsync(targetContent.Id));
                    }
                    else
                    {
                        result.List.Add(targetContent);
                    }
                }

                paginations.Add(result);
            }
            else
            {
                result = paginations.ElementAt(current.Page);
            }

            return result;
        }

        public SmartSolucionesCuba.SAPRESSC.Core.Common.Models.PaginationModel<Cloud.Api.Common.Models.ContentDisplayModel> GetPrevious(SmartSolucionesCuba.SAPRESSC.Core.Common.Models.PaginationModel<Cloud.Api.Common.Models.ContentDisplayModel> current)
        {
            return paginations.ElementAt(current.Page - 2);
        }
    }
}
