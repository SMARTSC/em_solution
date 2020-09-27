namespace EM.Cloud.Persistence.Entities
{
    public class PlacingEventClientBridge
    {
        public System.Guid EventId { get; set; }

        public Event Event { get; set; }

        public System.Guid ClientId { get; set; }

        public OrganizationClient Client { get; set; }
    }
}
