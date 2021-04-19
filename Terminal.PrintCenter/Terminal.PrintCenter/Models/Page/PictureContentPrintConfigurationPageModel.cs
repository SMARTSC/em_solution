namespace EM.Terminal.PrintCenter.Models.Page
{
    public class PictureContentPrintConfigurationPageModel : AbstractContentPrintConfigurationPageModel
    {
        public Cloud.Api.Common.Models.PictureContentDisplayModel Content { get; set; }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            Content = null;
        }
    }
}