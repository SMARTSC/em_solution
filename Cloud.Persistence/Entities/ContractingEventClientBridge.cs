namespace EM.Cloud.Persistence.Entities
{
    public class ContractingEventClientBridge
    {
        public System.Guid EventId { get; set; }

        public Event Event { get; set; }

        public System.Guid ClientId { get; set; }

        public Client Client { get; set; }
    }
}
