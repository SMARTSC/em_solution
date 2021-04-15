namespace EM.Extend.Layout.Provision.Providers
{
    public sealed class PrintLayoutDescriptorProvider
    {
        private static PrintLayoutDescriptorProvider instance;

        public static PrintLayoutDescriptorProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PrintLayoutDescriptorProvider();
                }

                return instance;
            }
        }

        private PrintLayoutDescriptorProvider() {}

        public Descriptors.IPrintLayoutDescriptor[] GetPrintLayoutDescriptors()
        {
            return new Descriptors.IPrintLayoutDescriptor[]
            {
                new Pioneer.Descriptors.Default_0001_1_3x2_PrintLayoutDescriptor(),
                new Pioneer.Descriptors.Default_0002_3_3x2_PrintLayoutDescriptor(),
                new Pioneer.Descriptors.Default_0003_1_3x2_PrintLayoutDescriptor()
            };
        }

        public Descriptors.IPrintLayoutDescriptor GetPrintLayoutDescriptorById(System.Guid id)
        {
            if (Pioneer.Descriptors.Default_0001_1_3x2_PrintLayoutDescriptor.ID == id)
            {
                return new Pioneer.Descriptors.Default_0001_1_3x2_PrintLayoutDescriptor();
            }

            if (Pioneer.Descriptors.Default_0002_3_3x2_PrintLayoutDescriptor.ID == id)
            {
                return new Pioneer.Descriptors.Default_0002_3_3x2_PrintLayoutDescriptor();
            }

            if (Pioneer.Descriptors.Default_0003_1_3x2_PrintLayoutDescriptor.ID == id)
            {
                return new Pioneer.Descriptors.Default_0003_1_3x2_PrintLayoutDescriptor();
            }

            return null;
        }
    }
}
