namespace EM.Cloud.Models.View
{
    public class EventAlbumLocksViewModel
    {
        public EventAlbumLocksViewModel()
        {
            Locks = new System.Collections.Generic.List<AlbumLockDisplayViewModel>();
        }

        public AlbumDisplayViewModel Album { get; set; }

        public System.Collections.Generic.List<AlbumLockDisplayViewModel> Locks { get; set; }
    }
}
