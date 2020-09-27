namespace EM.Cloud.Models.View
{
    public class TerminalDisplayViewModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Terminal, string>
    {
        public TerminalDisplayViewModel()
        {
            Albums = new System.Collections.Generic.List<AlbumDisplayViewModel>();
        }

        public string Id { get; set; }

        public bool Enabled { get; set; }

        public int Version { get; set; }

        public string DeviceBrand { get; set; }

        public string DeviceModel { get; set; }

        public string DeviceOS { get; set; }

        public string DeviceOSVersion { get; set; }

        public System.Collections.Generic.List<AlbumDisplayViewModel> Albums { get; set; }

        public void Import(Persistence.Entities.Terminal entity)
        {
            Id = entity.Id;
            Enabled = entity.Enabled;
            Version = entity.Version;
            DeviceBrand = entity.DeviceBrand;
            DeviceModel = entity.DeviceModel;
            DeviceOS = entity.DeviceOS;
            DeviceOSVersion = entity.DeviceOSVersion;
        }
    }
}
