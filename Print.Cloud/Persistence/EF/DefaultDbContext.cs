using Microsoft.EntityFrameworkCore;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Providers.Facade;
using System.Collections.Generic;

namespace EM.Print.Cloud.Persistence.EF
{
    public class DefaultDbContext : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Context.AbstractTraceableDbContext
    {
        public DefaultDbContext(IEnumerable<IDbSchemaConfigurationProviderFacade> dbSchemaConfigurationProviders, DbContextOptions options) : base(dbSchemaConfigurationProviders, options)
        {
        }

        public override bool TraceChanges => false;

        public DbSet<Print.Persistence.Entities.PrintOrder> PrintOrders { get; set; }
    }
}
