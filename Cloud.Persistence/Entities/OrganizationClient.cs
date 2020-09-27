namespace EM.Cloud.Persistence.Entities
{
    public class OrganizationClient : Client
    {
        public OrganizationClient()
        {
            Identity = new SmartSolucionesCuba.SAPRESSC.Middleware.Identity.Persistence.Entities.OrganizationIdentity();
        }

        public SmartSolucionesCuba.SAPRESSC.Middleware.Identity.Persistence.Entities.OrganizationIdentity Identity { get; set; }

        public System.Guid OrganizationIdentityId { get; set; }
    }
}
