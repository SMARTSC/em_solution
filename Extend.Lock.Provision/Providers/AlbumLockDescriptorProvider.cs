namespace EM.Extend.Lock.Provision.Providers
{
    public class AlbumLockDescriptorProvider
    {
        private static AlbumLockDescriptorProvider instance;

        public static AlbumLockDescriptorProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AlbumLockDescriptorProvider();
                }

                return instance;
            }
        }

        private AlbumLockDescriptorProvider() { }

        public Descriptors.IAlbumLockDescriptor[] GetAllDescriptors()
        {
            return new Descriptors.IAlbumLockDescriptor[]
            {
                new Pioneer.Descriptors.PasswordAlbumLockDescriptor()
            };
        }

        public Descriptors.IAlbumLockDescriptor GetDescriptorById(System.Guid id)
        {
            if (Pioneer.Descriptors.PasswordAlbumLockDescriptor.ID == id)
            {
                return new Pioneer.Descriptors.PasswordAlbumLockDescriptor();
            }

            return null;
        }
    }
}
