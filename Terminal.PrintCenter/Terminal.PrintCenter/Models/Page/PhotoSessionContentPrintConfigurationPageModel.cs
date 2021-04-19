namespace EM.Terminal.PrintCenter.Models.Page
{
    public class PhotoSessionContentPrintConfigurationPageModel : AbstractContentPrintConfigurationPageModel
    {
        public Cloud.Api.Common.Models.PhotoSessionContentDisplayModel Content { get; set; }

        public int[] Permutation { get; set; }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            Content = null;

            Permutation = null;
        }
    }
}
