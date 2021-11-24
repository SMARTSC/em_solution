namespace EM.Terminal.PrintCenter.Providers
{
    public class PrintLayoutViewProvider
    {
        public Views.Layout.Print.BasePictureContentPrintLayoutView GetPictureContentPrintLayoutView(Cloud.Api.Common.Models.PictureContentDisplayModel model, Extend.Layout.Descriptors.IPrintLayoutDescriptor descriptor, System.Action presentationFailCallbackAction = null, System.Action presentationReadyCallbackAction = null)
        {
            if (descriptor.Id == Extend.Layout.Pioneer.Descriptors.Default_0001_1_3x2_PrintLayoutDescriptor.ID)
            {
                return new Views.Layout.Print.Default_0001_1_3x2_PrintLayoutView(model, presentationReadyCallbackAction, presentationFailCallbackAction);
            }

            if (descriptor.Id == Extend.Layout.Pioneer.Descriptors.Default_0003_1_3x2_PrintLayoutDescriptor.ID)
            {
                return new Views.Layout.Print.Default_0003_1_3x2_PrintLayoutView(model, presentationReadyCallbackAction, presentationFailCallbackAction);
            }

            throw new System.InvalidOperationException();
        }

        public Views.Layout.Print.BasePhotoSessionContentPrintLayoutView GetPhotoSessionContentPrintLayoutView(Cloud.Api.Common.Models.PhotoSessionContentDisplayModel model, int[] permutation, Extend.Layout.Descriptors.IPrintLayoutDescriptor descriptor, System.Action presentationFailCallbackAction = null, System.Action presentationReadyCallbackAction = null)
        {
            if (descriptor.Id == Extend.Layout.Pioneer.Descriptors.Default_0002_3_3x2_PrintLayoutDescriptor.ID)
            {
                return new Views.Layout.Print.Default_0002_3_3x2_PrintLayoutView(model, permutation, presentationReadyCallbackAction, presentationFailCallbackAction);
            }

            throw new System.InvalidOperationException();
        }
    }
}
