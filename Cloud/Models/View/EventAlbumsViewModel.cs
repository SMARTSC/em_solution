namespace EM.Cloud.Models.View
{
    public class EventAlbumsViewModel : AbstractEventRelationViewModel
    {
        public Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] DefaultPictureContentCollageLayoutSelectListItems { get; set; }

        public Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] DefaultPhotoSessionContentCollageLayoutSelectListItems { get; set; }

        public System.Collections.Generic.List<AlbumDisplayViewModel> Albums { get; set; }
    }
}