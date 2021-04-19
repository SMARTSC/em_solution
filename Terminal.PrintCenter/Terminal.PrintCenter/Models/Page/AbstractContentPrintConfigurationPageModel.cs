namespace EM.Terminal.PrintCenter.Models.Page
{
    public abstract class AbstractContentPrintConfigurationPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private int printCopyCount;
        private Extend.Layout.Descriptors.IPrintLayoutDescriptor printLayoutDescriptor;

        public AbstractContentPrintConfigurationPageModel()
        {
            printCopyCount = 1;
        }

        public int PrintCopyCount { get => printCopyCount; set => SetProperty(ref printCopyCount, value); }

        public Extend.Layout.Descriptors.IPrintLayoutDescriptor PrintLayoutDescriptor { get => printLayoutDescriptor; set => SetProperty(ref printLayoutDescriptor, value); }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            PrintLayoutDescriptor = null;
        }
    }
}
