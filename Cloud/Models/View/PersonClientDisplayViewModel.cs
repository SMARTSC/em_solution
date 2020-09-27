using SmartSolucionesCuba.SAPRESSC.Middleware.Identity.Persistence.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EM.Cloud.Models.View
{
    public class PersonClientDisplayViewModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.PersonClient, System.Guid>
    {
        public System.Guid Id { get; set; }

        public System.Guid ImageId { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public System.DateTime BornDate { get; set; }
        
        public SmartSolucionesCuba.SAPRESSC.Middleware.Identity.Persistence.Enumerations.Sex Sex { get; set; }

        public SmartSolucionesCuba.SAPRESSC.Middleware.Identity.Persistence.Enumerations.CivilStatus CivilStatus { get; set; }

        public string Alias { get; set; }

        public string About { get; set; }

        public ICollection<EmailContactItem> EmailContactItems { get; set; }

        public ICollection<PhoneContactItem> PhoneContactItems { get; set; }

        public ICollection<PostalContactItem> PostalContactItems { get; set; }

        public ICollection<SocialContactItem> SocialContactItems { get; set; }

        public ICollection<WebSiteContactItem> WebSiteContactItems { get; set; }

        public string Notes { get; set; }

        public void Import(Persistence.Entities.PersonClient entity)
        {
            Id = entity.Id;
            ImageId = entity.Identity.Image != null? entity.Identity.Image.Id : System.Guid.Empty;
            Name = entity.Identity.Name;
            LastName = entity.Identity.LastName;
            BornDate = entity.Identity.BornDate;
            Sex = entity.Identity.Sex;
            CivilStatus = entity.Identity.CivilStatus;
            Alias = entity.Identity.Alias;
            About = entity.Identity.About;
            EmailContactItems = entity.Identity.PersonEmailContactItemBridges.Select(bridge => bridge.ContactItem).ToArray();
            PhoneContactItems = entity.Identity.PersonPhoneContactItemBridges.Select(bridge => bridge.ContactItem).ToArray();
            PostalContactItems = entity.Identity.PersonPostalContactItemBridges.Select(bridge => bridge.ContactItem).ToArray();
            SocialContactItems = entity.Identity.PersonSocialContactItemBridges.Select(bridge => bridge.ContactItem).ToArray();
            WebSiteContactItems = entity.Identity.PersonWebSiteContactItemBridges.Select(bridge => bridge.ContactItem).ToArray();
            Notes = entity.Notes;
        }
    }
}
