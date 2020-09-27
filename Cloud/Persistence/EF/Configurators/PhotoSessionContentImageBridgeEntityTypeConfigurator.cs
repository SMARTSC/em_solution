namespace EM.Cloud.Persistence.EF.Configurators
{
    public class PhotoSessionContentImageBridgeEntityTypeConfigurator : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Configurators.AbstractEntityTypeConfigurator<Entities.PhotoSessionContentImageBridge>
    {
        protected override void ApplayRules(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.PhotoSessionContentImageBridge> entityTypeBuilder)
        {
            base.ApplayRules(entityTypeBuilder);

            entityTypeBuilder.HasKey(bridge => new { bridge.PhotoSessionId, bridge.ImageId });

            entityTypeBuilder.HasOne(bridge => bridge.PhotoSession).WithMany(entity => entity.ImageBridges).HasForeignKey(bridge => bridge.PhotoSessionId);
            entityTypeBuilder.HasOne(bridge => bridge.Image).WithMany().HasForeignKey(bridge => bridge.ImageId);
        }
    }
}
