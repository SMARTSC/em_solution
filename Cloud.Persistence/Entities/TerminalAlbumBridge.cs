namespace EM.Cloud.Persistence.Entities
{
    public class TerminalAlbumBridge
    {
        public string TerminalId { get; set; }

        public Terminal Terminal { get; set; }

        public System.Guid AlbumId { get; set; }

        public Album Album { get; set; }
    }
}
