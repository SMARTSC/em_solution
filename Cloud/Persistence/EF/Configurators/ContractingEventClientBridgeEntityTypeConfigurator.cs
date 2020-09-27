namespace EM.Cloud.Persistence.EF.Configurators
{
    public class ContractingEventClientBridgeEntityTypeConfigurator : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Configurators.AbstractEntityTypeConfigurator<Entities.ContractingEventClientBridge>
    {
        protected override void ApplayRules(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.ContractingEventClientBridge> entityTypeBuilder)
        {
            base.ApplayRules(entityTypeBuilder);

            entityTypeBuilder.HasKey(bridge => new { bridge.EventId, bridge.ClientId });

            entityTypeBuilder.HasOne(bridge => bridge.Client).WithMany().HasForeignKey(bridge => bridge.ClientId);
            entityTypeBuilder.HasOne(bridge => bridge.Event).WithMany(entity => entity.ContractingClientBridges).HasForeignKey(bridge => bridge.EventId);
        }
    }
}
