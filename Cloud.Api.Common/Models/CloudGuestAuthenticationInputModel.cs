using System.ComponentModel.DataAnnotations;

namespace EM.Cloud.Api.Common.Models
{
    public class CloudGuestAuthenticationInputModel
    {
        [Required]
        public string TerminalId { get; set; }

        [Required]
        public int Version { get; set; }

        [Required]
        public string DeviceBrand { get; set; }

        [Required]
        public string DeviceModel { get; set; }

        [Required]
        public string DeviceOS { get; set; }

        [Required]
        public string DeviceOSVersion { get; set; }
    }
}
