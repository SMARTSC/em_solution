namespace EM.Cloud.Persistence.Entities
{
    public class AlbumLockBridge
    {
        public System.Guid AlbumId { get; set; }

        public Album Album { get; set; }

        public System.Guid LockId { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string SerializedContext { get; set; }
    }
}
