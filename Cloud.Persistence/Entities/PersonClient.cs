namespace EM.Cloud.Persistence.Entities
{
    public class PersonClient : Client
    {
        public PersonClient()
        {
            Identity = new SmartSolucionesCuba.SAPRESSC.Middleware.Identity.Persistence.Entities.PersonIdentity();
        }

        public SmartSolucionesCuba.SAPRESSC.Middleware.Identity.Persistence.Entities.PersonIdentity Identity { get; set; }

        public System.Guid PersonIdentityId { get; set; }
    }
}
