namespace EM.Terminal.PrintCenter.Pages
{
    public interface IPhotoSessionContentPrintableViewPresenter
    {
        Cloud.Api.Common.Models.PhotoSessionContentDisplayModel PhotoSessionContent { get; }

        int[] Permutation { get; }

        System.Action PrintableViewPresentationFailCallbackAction { get; }

        System.Action PrintableViewPresentationReadyCallbackAction { get; }
    }
}
