namespace EM.Cloud.Persistence.EF.Configurators
{
    public class AnimateContentImageBridgeEntityTypeConfigurator : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Configurators.AbstractEntityTypeConfigurator<Entities.AnimateContentImageBridge>
    {
        protected override void ApplayRules(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.AnimateContentImageBridge> entityTypeBuilder)
        {
            base.ApplayRules(entityTypeBuilder);

            entityTypeBuilder.HasKey(bridge => new { bridge.AnimateId, bridge.ImageId });

            entityTypeBuilder.HasOne(bridge => bridge.Animate).WithMany(entity => entity.ImageBridges).HasForeignKey(bridge => bridge.AnimateId);
            entityTypeBuilder.HasOne(bridge => bridge.Image).WithMany().HasForeignKey(bridge => bridge.ImageId);
        }
    }
}
