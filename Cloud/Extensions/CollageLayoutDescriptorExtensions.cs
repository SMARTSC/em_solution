namespace EM.Cloud.Extensions
{
    public static class CollageLayoutDescriptorExtensions
    {
        public static Persistence.Entities.CollageLayout ToEntity(this Extend.Layout.Descriptors.Facade.ICollageLayoutDescriptorFacade target)
        {
            return new Persistence.Entities.CollageLayout
            {
                Id = target.Id
            };
        }
    }
}