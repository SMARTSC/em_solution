namespace EM.Extend.Lock.Descriptors
{
    public interface IAlbumLockDescriptor
    {
        System.Guid Id { get; }

        string Name { get; }

        string Description { get; }
    }
}
