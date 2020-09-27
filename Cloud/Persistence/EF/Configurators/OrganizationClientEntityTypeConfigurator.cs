namespace EM.Cloud.Persistence.EF.Configurators
{
    public class OrganizationClientEntityTypeConfigurator : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Configurators.AbstractEntityTypeConfigurator<Entities.OrganizationClient>
    {
        protected override void ApplayRules(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.OrganizationClient> entityTypeBuilder)
        {
            base.ApplayRules(entityTypeBuilder);

            entityTypeBuilder.HasOne(entity => entity.Identity).WithMany().HasForeignKey(entity => entity.OrganizationIdentityId);
        }
    }
}
