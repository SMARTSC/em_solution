using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EM.Cloud.Persistence.EF.Configurators
{
    public class AlbumEntityTypeConfigurator : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Configurators.AbstractEntityTypeConfigurator<Entities.Album>
    {
        protected override void ApplayRules(EntityTypeBuilder<Entities.Album> entityTypeBuilder)
        {
            base.ApplayRules(entityTypeBuilder);

            entityTypeBuilder.HasIndex(e => e.Alias).IsUnique();
        }
    }
}
