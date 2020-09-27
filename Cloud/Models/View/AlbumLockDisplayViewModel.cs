namespace EM.Cloud.Models.View
{
    public class AlbumLockDisplayViewModel
    {
        public Extend.Lock.Descriptors.Facade.IAlbumLockDescriptorFacade Descriptor { get; set; }

        public string SerializedContext { get; set; }
    }
}
