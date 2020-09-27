namespace EM.Cloud.Persistence.EF.Configurators
{
    public class AlbumCollageLayoutBridgeEntityTypeConfigurator : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Configurators.AbstractEntityTypeConfigurator<Entities.AlbumCollageLayoutBridge>
    {
        protected override void ApplayRules(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.AlbumCollageLayoutBridge> entityTypeBuilder)
        {
            base.ApplayRules(entityTypeBuilder);

            entityTypeBuilder.HasKey(bridge => new { bridge.AlbumId, bridge.CollageLayoutId  });

            entityTypeBuilder.HasOne(bridge => bridge.Album).WithMany(entity => entity.CollageLayoutBridges).HasForeignKey(bridge => bridge.AlbumId);
            entityTypeBuilder.HasOne(bridge => bridge.CollageLayout).WithMany().HasForeignKey(bridge => bridge.CollageLayoutId);
        }
    }
}
