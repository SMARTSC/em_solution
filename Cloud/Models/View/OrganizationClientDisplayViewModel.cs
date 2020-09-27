using SmartSolucionesCuba.SAPRESSC.Middleware.Identity.Persistence.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EM.Cloud.Models.View
{
    public class OrganizationClientDisplayViewModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.OrganizationClient, System.Guid>
    {
        public System.Guid Id { get; set; }

        public System.Guid ImageId { get; set; }

        public string Name { get; set; }

        public System.DateTime BornDate { get; set; }

        public string About { get; set; }

        public ICollection<EmailContactItem> EmailContactItems { get; set; }

        public ICollection<PhoneContactItem> PhoneContactItems { get; set; }

        public ICollection<PostalContactItem> PostalContactItems { get; set; }

        public ICollection<SocialContactItem> SocialContactItems { get; set; }

        public ICollection<WebSiteContactItem> WebSiteContactItems { get; set; }

        public string Notes { get; set; }

        public void Import(Persistence.Entities.OrganizationClient entity)
        {
            Id = entity.Id;
            ImageId = entity.Identity.Image != null ? entity.Identity.Image.Id : System.Guid.Empty;
            Name = entity.Identity.Name;
            BornDate = entity.Identity.BornDate;
            About = entity.Identity.About;
            EmailContactItems = entity.Identity.OrganizationEmailContactItemBridges.Select(bridge => bridge.ContactItem).ToArray();
            PhoneContactItems = entity.Identity.OrganizationPhoneContactItemBridges.Select(bridge => bridge.ContactItem).ToArray();
            PostalContactItems = entity.Identity.OrganizationPostalContactItemBridges.Select(bridge => bridge.ContactItem).ToArray();
            SocialContactItems = entity.Identity.OrganizationSocialContactItemBridges.Select(bridge => bridge.ContactItem).ToArray();
            WebSiteContactItems = entity.Identity.OrganizationWebSiteContactItemBridges.Select(bridge => bridge.ContactItem).ToArray();
            Notes = entity.Notes;
        }
    }
}
