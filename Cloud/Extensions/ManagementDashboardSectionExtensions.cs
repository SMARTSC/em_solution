namespace EM.Cloud.Extensions
{
    public static class ManagementDashboardSectionExtensions
    {
        public static Microsoft.AspNetCore.Html.HtmlString ToIconographyHtmlString(this SmartSolucionesCuba.SAPRESSC.Core.Web.Backend.Models.View.ManagementDashboardSectionViewModel self)
        {
            if (self.Controller == "users") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"fas fa-user\"></span>");

            if (self.Controller == "groups") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"fas fa-users\"></span>");

            if (self.Controller == "traces") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"fas fa-shoe-prints\"></span>");

            if (self.Controller.EndsWith("clients")) return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"fas fa-user-tie\"></span>");

            if (self.Controller == "collagelayouts") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"fas fa-th-large\"></span>");

            if (self.Controller == "terminals") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"fas fa-mobile\"></span>");

            if (self.Controller == "invokations") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"fas fa-satellite\"></span>");

            return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"fas fa-certificate\"></span>");
        }
    }
}
