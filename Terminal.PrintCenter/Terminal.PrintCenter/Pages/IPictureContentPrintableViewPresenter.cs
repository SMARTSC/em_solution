namespace EM.Terminal.PrintCenter.Pages
{
    public interface IPictureContentPrintableViewPresenter
    {
        Cloud.Api.Common.Models.PictureContentDisplayModel PictureContent { get; }

        System.Action PrintableViewPresentationFailCallbackAction { get; } 
        
        System.Action PrintableViewPresentationReadyCallbackAction { get; }
    }
}
