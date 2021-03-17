namespace EM.Terminal.Dashboard.Factories.Model.Page
{
    public static class EventKindConfigurationPageModelFactory
    {
        public static Models.Page.EventKindConfigurationPageModel Build(Cloud.Backend.Api.Common.Models.EventKindDisplayModel model)
        {
            var result = new Models.Page.EventKindConfigurationPageModel();

            if (model != null)
            {
                result.Id = model.Id;
                result.Name = model.Name;
            }

            return result;
        }
    }
}
