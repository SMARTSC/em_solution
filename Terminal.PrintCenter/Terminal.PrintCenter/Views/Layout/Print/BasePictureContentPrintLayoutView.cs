namespace EM.Terminal.PrintCenter.Views.Layout.Print
{
    public class BasePictureContentPrintLayoutView : AbstractPrintLayoutView
    {
        public BasePictureContentPrintLayoutView()
        {
            throw new System.InvalidOperationException();
        }

        public BasePictureContentPrintLayoutView(Extend.Layout.Descriptors.IPrintLayoutDescriptor descriptor, System.Action presentationReadyCallbackAction, System.Action presentationFailCallbackAction) 
            : base(descriptor, presentationReadyCallbackAction, presentationFailCallbackAction)
        {
        }

        protected void CachedImage_Success(object sender, FFImageLoading.Forms.CachedImageEvents.SuccessEventArgs e)
        {
            (sender as FFImageLoading.Forms.CachedImage).Success -= CachedImage_Success;
            (sender as FFImageLoading.Forms.CachedImage).Error -= CachedImage_Error;

            presentationReadyCallbackAction?.Invoke();
        }

        protected void CachedImage_Error(object sender, FFImageLoading.Forms.CachedImageEvents.ErrorEventArgs e)
        {
            (sender as FFImageLoading.Forms.CachedImage).Success -= CachedImage_Success;
            (sender as FFImageLoading.Forms.CachedImage).Error -= CachedImage_Error;

            presentationFailCallbackAction?.Invoke();
        }
    }
}
