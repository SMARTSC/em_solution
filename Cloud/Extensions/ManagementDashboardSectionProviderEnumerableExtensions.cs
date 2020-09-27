using EM.Cloud.Security.Authorization;
using SmartSolucionesCuba.SAPRESSC.Core.Web.Backend.Models.View;
using System.Collections.Generic;

namespace EM.Cloud.Extensions
{
    public static class ManagementDashboardSectionProviderEnumerableExtensions
    {
        public static Dictionary<string, ICollection<ManagementDashboardSectionViewModel>> ToManagementDashboardSectionGroups(this IEnumerable<SmartSolucionesCuba.SAPRESSC.Core.Web.Backend.Providers.Facade.IManagementDashboardSectionProviderFacade> self)
        {
            var securitySections = new List<ManagementDashboardSectionViewModel>();
            var businessSections = new List<ManagementDashboardSectionViewModel>();
            var supportSections = new List<ManagementDashboardSectionViewModel>();

            foreach (var provider in self)
            {
                if (provider is SmartSolucionesCuba.SAPRESSC.Core.Web.SecurityLocal.Providers.LocalSecurityWebManagementDashboardSectionProvider)
                {
                    securitySections.AddRange(provider.GetManagementDashboardSections());
                }
                else if (provider is SmartSolucionesCuba.SAPRESSC.Middleware.Region.Web.Backend.Providers.MiddlewareRegionWebBackendManagementDashboardSectionProvider)
                {
                    supportSections.AddRange(provider.GetManagementDashboardSections());
                }
            }

            businessSections.Add(new ManagementDashboardSectionViewModel
            {
                Title = "clients",
                Controller = "personclients",
                Action = "index",
                Area = "business",
                Role = Roles.ClientViewer
            });
            businessSections.Add(new ManagementDashboardSectionViewModel
            {
                Title = "events",
                Controller = "events",
                Action = "index",
                Area = "business",
                Role = Roles.EventViewer
            });
            businessSections.Add(new ManagementDashboardSectionViewModel
            {
                Title = "terminals",
                Controller = "terminals",
                Action = "index",
                Area = "business",
                Role = Roles.TerminalViewer
            });
            businessSections.Add(new ManagementDashboardSectionViewModel
            {
                Title = "invokations",
                Controller = "invokations",
                Action = "index",
                Area = "business",
                Role = Roles.InvocationViewer
            });

            supportSections.Insert(0, new ManagementDashboardSectionViewModel
            {
                Title = "collagelayouts",
                Controller = "collagelayouts",
                Action = "index",
                Area = "business",
                Role = Roles.CollageLayoutViewer
            });

            supportSections.Add(new ManagementDashboardSectionViewModel
            {
                Title = "printorders",
                Controller = "printorders",
                Action = "index",
                Area = "business",
                Role = Roles.PrintOrderViewer
            });

            securitySections.Add(new ManagementDashboardSectionViewModel
            {
                Title = "traces",
                Controller = "traces",
                Action = "index",
                Area = "security",
                Role = Roles.TraceViewer
            });

            return new Dictionary<string, ICollection<ManagementDashboardSectionViewModel>>
            {
                { "Seguridad", securitySections },
                { "Negocio", businessSections },
                { "Soporte", supportSections }
            };
        }
    }
}
