namespace EM.Terminal.Dashboard.Models.Page
{
    public class ManageEventAlbumLocksPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private Cloud.Backend.Api.Common.Models.AlbumLockDisplayModel passwordLockConfiguration;

        public ManageEventAlbumLocksPageModel()
        {
            PasswordLockDescriptor = Extend.Lock.Provision.Providers.AlbumLockDescriptorProvider.Instance.GetDescriptorById(Extend.Lock.Pioneer.Descriptors.PasswordAlbumLockDescriptor.ID);
        }

        public Cloud.Backend.Api.Common.Models.AlbumDisplayModel Album { get; set; }

        public Extend.Lock.Descriptors.IAlbumLockDescriptor PasswordLockDescriptor { get; private set; }

        public Cloud.Backend.Api.Common.Models.AlbumLockDisplayModel PasswordLockConfiguration { get => passwordLockConfiguration; set => SetProperty(ref passwordLockConfiguration, value); }
    }
}
