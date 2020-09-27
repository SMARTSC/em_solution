using System.ComponentModel.DataAnnotations;

namespace EM.Cloud.Persistence.Entities
{
    public class Terminal : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Entities.IHabilitableEntity<string>
    {
        public Terminal()
        {
            AlbumBridges = new System.Collections.Generic.List<TerminalAlbumBridge>();
        }

        [Key]
        public string Id { get; set; }

        public bool Enabled { get; set; }

        public int Version { get; set; }

        public string DeviceBrand { get; set; }

        public string DeviceModel { get; set; }

        public string DeviceOS { get; set; }

        public string DeviceOSVersion { get; set; }

        public System.Collections.Generic.List<TerminalAlbumBridge> AlbumBridges { get; set; }
    }
}
