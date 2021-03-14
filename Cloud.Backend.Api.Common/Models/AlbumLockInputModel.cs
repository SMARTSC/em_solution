namespace EM.Cloud.Backend.Api.Common.Models
{
    public class AlbumLockInputModel
    {
        public System.Guid AlbumId { get; set; }

        public System.Guid LockId { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string SerializedContext { get; set; }
    }
}
