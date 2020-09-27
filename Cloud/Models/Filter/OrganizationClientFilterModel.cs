namespace EM.Cloud.Models.Filter
{
    public class OrganizationClientFilterModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.Filter.IEntityFilterModel<Persistence.Entities.OrganizationClient, System.Guid>
    {
        public string Name { get; set; }

        public System.Linq.Expressions.Expression<System.Func<Persistence.Entities.OrganizationClient, bool>> GetSelector()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name)) return null;

            return entity => entity.Identity.Name.Contains(Name);
        }
    }
}
