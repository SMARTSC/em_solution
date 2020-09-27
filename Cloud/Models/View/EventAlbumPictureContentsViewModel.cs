namespace EM.Cloud.Models.View
{
    public class EventAlbumPictureContentsViewModel
    {
        public EventAlbumPictureContentsViewModel()
        {
            Contents = new System.Collections.Generic.List<Persistence.Entities.PictureContent>();
        }

        public AlbumDisplayViewModel Album { get; set; }

        public System.Collections.Generic.List<Persistence.Entities.PictureContent> Contents { get; set; }
    }
}
