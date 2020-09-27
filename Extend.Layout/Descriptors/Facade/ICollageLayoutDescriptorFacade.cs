namespace EM.Extend.Layout.Descriptors.Facade
{
    public interface ICollageLayoutDescriptorFacade
    {
        System.Guid Id { get; }

        string Name { get; }

        int PictureCount { get; }

        string Description { get; }
    }
}
