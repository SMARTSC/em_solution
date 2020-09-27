namespace EM.Cloud.Persistence.Entities
{
    public class AlbumCollageLayoutBridge
    {
        public System.Guid AlbumId { get; set; }

        public Album Album { get; set; }

        public System.Guid CollageLayoutId { get; set; }

        public CollageLayout CollageLayout { get; set; }
    }
}
