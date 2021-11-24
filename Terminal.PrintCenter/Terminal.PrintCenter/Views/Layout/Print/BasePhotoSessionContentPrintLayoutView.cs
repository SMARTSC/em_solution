namespace EM.Terminal.PrintCenter.Views.Layout.Print
{
    public class BasePhotoSessionContentPrintLayoutView : AbstractPrintLayoutView
    {
        protected int[] permutation;

        public BasePhotoSessionContentPrintLayoutView()
        {
            throw new System.InvalidOperationException();
        }

        public BasePhotoSessionContentPrintLayoutView(int[] permutation, System.Action presentationReadyCallbackAction, System.Action presentationFailCallbackAction) 
            : base(new Extend.Layout.Pioneer.Descriptors.Default_0002_3_3x2_PrintLayoutDescriptor(), presentationReadyCallbackAction, presentationFailCallbackAction)
        {
            this.permutation = permutation;
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            permutation = null;
        }
    }
}
