namespace EM.Cloud.Models.Filter
{
    public enum CompletitionStateFilteringOption
    {
        Eny,
        Attended,
        Unattended
    }

    public enum EnabledStateFilteringOption
    {
        Eny,
        Enabled,
        Disabled
    }

    public class PrintOrderFilterModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.Filter.IEntityFilterModel<Persistence.Entities.PrintOrder, long>
    {
        [System.ComponentModel.DataAnnotations.Display(Name = "CompletitionState")]
        public CompletitionStateFilteringOption CompletitionState { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "EnabledState")]
        public EnabledStateFilteringOption EnabledState { get; set; }

        public PrintOrderFilterModel()
        {
            CompletitionState = CompletitionStateFilteringOption.Eny;
            EnabledState = EnabledStateFilteringOption.Eny;
        }

        public System.Linq.Expressions.Expression<System.Func<Persistence.Entities.PrintOrder, bool>> GetSelector()
        {
            if (CompletitionState != CompletitionStateFilteringOption.Eny || EnabledState != EnabledStateFilteringOption.Eny)
            {
                switch (CompletitionState)
                {
                    case CompletitionStateFilteringOption.Attended:
                        if (EnabledState == EnabledStateFilteringOption.Eny)
                        {
                            return entity => entity.Created <= entity.Attended;
                        }

                        return entity => entity.Created <= entity.Attended && entity.Enabled == (EnabledState == EnabledStateFilteringOption.Enabled);

                    case CompletitionStateFilteringOption.Unattended:
                        if (EnabledState == EnabledStateFilteringOption.Eny)
                        {
                            return entity => entity.Created > entity.Attended;
                        }

                        return entity => entity.Created > entity.Attended && entity.Enabled == (EnabledState == EnabledStateFilteringOption.Enabled);

                    default:
                        return entity => entity.Enabled == (EnabledState == EnabledStateFilteringOption.Enabled);
                }
            }

            return entity => true;
        }
    }
}
