namespace EM.Terminal.PrintCenter.Extensions
{
    public static class PrintLayoutDescriptorExtensions
    {
        public static int[] ToDefaultPermutation(this Extend.Layout.Descriptors.IPrintLayoutDescriptor descriptor)
        {
            var result = new int[descriptor.PictureCount];

            for (int i = 0; i < descriptor.PictureCount; i++)
            {
                result[i] = i;
            }

            return result;
        }
    }
}
