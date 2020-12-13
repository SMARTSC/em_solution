namespace EM.Cloud.Backend.Api.Common.Models
{
    public class ImageMediaInputModel
    {
        [System.ComponentModel.DataAnnotations.Required]
        public byte[] ContentData { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string ContentType { get; set; }
    }
}
