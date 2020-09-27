namespace EM.Cloud.Persistence.EF.Configurators
{
    public class TerminalAlbumBridgeEntityTypeConfigurator : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Configurators.AbstractEntityTypeConfigurator<Entities.TerminalAlbumBridge>
    {
        protected override void ApplayRules(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.TerminalAlbumBridge> entityTypeBuilder)
        {
            base.ApplayRules(entityTypeBuilder);

            entityTypeBuilder.HasKey(bridge => new { bridge.TerminalId, bridge.AlbumId });

            entityTypeBuilder.HasOne(bridge => bridge.Terminal).WithMany(entity => entity.AlbumBridges).HasForeignKey(bridge => bridge.TerminalId);
            entityTypeBuilder.HasOne(bridge => bridge.Album).WithMany().HasForeignKey(bridge => bridge.AlbumId);
        }
    }
}
