namespace EM.Cloud.Models.View
{
    public class CollageLayoutDisplayViewModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.CollageLayout, System.Guid>
    {
        public System.Guid Id { get; set; }

        public bool Enabled { get; set; }

        public string Name { get; set; }

        public int PictureCount { get; set; }

        public string Description { get; set; }

        public void Import(Persistence.Entities.CollageLayout entity)
        {
            Id = entity.Id;
            Enabled = entity.Enabled;

            var descriptor = Extend.Layout.Provision.Providers.CollageLayoutDescriptorProvider.Instance.GetCollageLayoutDescriptorById(entity.Id);

            Name = descriptor.Name;
            PictureCount = descriptor.PictureCount;
            Description = descriptor.Description;
        }
    }
}
