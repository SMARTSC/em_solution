namespace EM.Print.Api.Common.Models.Filter
{
    public class PrintOrderFilterModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.Filter.IEntityFilterModel<Persistence.Entities.PrintOrder, long>
    {
        public bool PendingOnly { get; set; }

        public bool EnabledOnly { get; set; }

        public System.Linq.Expressions.Expression<System.Func<Persistence.Entities.PrintOrder, bool>> GetSelector()
        {
            if (PendingOnly && EnabledOnly)
            {
                return entity => entity.Enabled && entity.Created >= entity.Attended;
            }

            if (PendingOnly)
            {
                return entity => entity.Created >= entity.Attended;
            }

            if (EnabledOnly)
            {
                return entity => entity.Enabled;
            }

            return entity => true;
        }
    }
}
