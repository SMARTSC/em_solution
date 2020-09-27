namespace EM.Cloud.Persistence.EF.Configurators
{
    public class AlbumLockBridgeEntityTypeConfigurator : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Configurators.AbstractEntityTypeConfigurator<Entities.AlbumLockBridge>
    {
        protected override void ApplayRules(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.AlbumLockBridge> entityTypeBuilder)
        {
            base.ApplayRules(entityTypeBuilder);

            entityTypeBuilder.HasKey(bridge => new { bridge.AlbumId, bridge.LockId });

            entityTypeBuilder.HasOne(bridge => bridge.Album).WithMany(entity => entity.LockBridges).HasForeignKey(bridge => bridge.AlbumId);
        }
    }
}
