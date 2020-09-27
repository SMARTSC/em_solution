namespace EM.Cloud.Models.View
{
    public class AlbumPasswordLockInputViewModel
    {
        public System.Guid AlbumId { get; set; }

        public System.Guid EventId { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.Display(Name = "Password")]
        public string Password { get; set; }
    }
}
