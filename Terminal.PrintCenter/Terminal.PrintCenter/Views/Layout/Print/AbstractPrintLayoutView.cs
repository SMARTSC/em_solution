namespace EM.Terminal.PrintCenter.Views.Layout.Print
{
    public abstract class AbstractPrintLayoutView : SmartSolucionesCuba.XFToolKit.Mvvm.Views.ContentView
    {
        protected bool forOverview;
        protected System.Action presentationReadyCallbackAction;
        protected System.Action presentationFailCallbackAction;

        public AbstractPrintLayoutView()
        {
            throw new System.InvalidOperationException();
        }

        public AbstractPrintLayoutView(Extend.Layout.Descriptors.IPrintLayoutDescriptor descriptor, bool forOverview, System.Action presentationReadyCallbackAction, System.Action presentationFailCallbackAction)
        {
            this.forOverview = forOverview;
            this.presentationReadyCallbackAction = presentationReadyCallbackAction;
            this.presentationFailCallbackAction = presentationFailCallbackAction;

            Descriptor = descriptor;
        }

        public Extend.Layout.Descriptors.IPrintLayoutDescriptor Descriptor { get; private set; }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            presentationReadyCallbackAction = null;
            presentationFailCallbackAction = null;
        }
    }
}
