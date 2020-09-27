namespace EM.Cloud.Models.Filter
{
    public class PersonClientFilterModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.Filter.IEntityFilterModel<Persistence.Entities.PersonClient, System.Guid>
    {
        public string Name { get; set; }

        public System.Linq.Expressions.Expression<System.Func<Persistence.Entities.PersonClient, bool>> GetSelector()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name)) return null;

            return entity => entity.Identity.Name.Contains(Name) || entity.Identity.LastName.Contains(Name);
        }
    }
}
