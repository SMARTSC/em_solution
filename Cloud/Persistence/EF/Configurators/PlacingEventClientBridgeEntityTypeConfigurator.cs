namespace EM.Cloud.Persistence.EF.Configurators
{
    public class PlacingEventClientBridgeEntityTypeConfigurator : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Configurators.AbstractEntityTypeConfigurator<Entities.PlacingEventClientBridge>
    {
        protected override void ApplayRules(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.PlacingEventClientBridge> entityTypeBuilder)
        {
            base.ApplayRules(entityTypeBuilder);

            entityTypeBuilder.HasKey(bridge => new { bridge.EventId, bridge.ClientId });

            entityTypeBuilder.HasOne(bridge => bridge.Client).WithMany().HasForeignKey(bridge => bridge.ClientId);
            entityTypeBuilder.HasOne(bridge => bridge.Event).WithMany(entity => entity.PlacingClientBridges).HasForeignKey(bridge => bridge.EventId);
        }
    }
}
