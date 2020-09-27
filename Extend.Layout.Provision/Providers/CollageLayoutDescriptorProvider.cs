namespace EM.Extend.Layout.Provision.Providers
{
    public sealed class CollageLayoutDescriptorProvider
    {
        private static CollageLayoutDescriptorProvider instance;

        public static CollageLayoutDescriptorProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CollageLayoutDescriptorProvider();
                }

                return instance;
            }
        }

        private CollageLayoutDescriptorProvider() {}

        public Descriptors.Facade.ICollageLayoutDescriptorFacade[] GetCollageLayoutDescriptors()
        {
            return new Descriptors.Facade.ICollageLayoutDescriptorFacade[]
            {
                new Pioneer.Descriptors.Default_0001_1_3x2_CollageLayoutDescriptor(),
                new Pioneer.Descriptors.Default_0002_3_3x2_CollageLayoutDescriptor(),
                new Pioneer.Descriptors.Default_0003_1_3x2_CollageLayoutDescriptor()
            };
        }

        public Descriptors.Facade.ICollageLayoutDescriptorFacade GetCollageLayoutDescriptorById(System.Guid id)
        {
            if (Pioneer.Descriptors.Default_0001_1_3x2_CollageLayoutDescriptor.ID == id)
            {
                return new Pioneer.Descriptors.Default_0001_1_3x2_CollageLayoutDescriptor();
            }

            if (Pioneer.Descriptors.Default_0002_3_3x2_CollageLayoutDescriptor.ID == id)
            {
                return new Pioneer.Descriptors.Default_0002_3_3x2_CollageLayoutDescriptor();
            }

            if (Pioneer.Descriptors.Default_0003_1_3x2_CollageLayoutDescriptor.ID == id)
            {
                return new Pioneer.Descriptors.Default_0003_1_3x2_CollageLayoutDescriptor();
            }

            return null;
        }
    }
}
