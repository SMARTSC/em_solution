namespace EM.Cloud.Persistence.EF.Configurators
{
    public class SponsoringEventClientBridgeEntityTypeConfigurator : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Configurators.AbstractEntityTypeConfigurator<Entities.SponsoringEventClientBridge>
    {
        protected override void ApplayRules(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.SponsoringEventClientBridge> entityTypeBuilder)
        {
            base.ApplayRules(entityTypeBuilder);

            entityTypeBuilder.HasKey(bridge => new { bridge.EventId, bridge.ClientId });

            entityTypeBuilder.HasOne(bridge => bridge.Client).WithMany().HasForeignKey(bridge => bridge.ClientId);
            entityTypeBuilder.HasOne(bridge => bridge.Event).WithMany(entity => entity.SponsoringClientBridges).HasForeignKey(bridge => bridge.EventId);
        }
    }
}
