namespace EM.Extend.Layout.Descriptors
{
    public interface IPrintLayoutDescriptor
    {
        System.Guid Id { get; }

        string Name { get; }

        int PictureCount { get; }

        string Description { get; }
    }
}
