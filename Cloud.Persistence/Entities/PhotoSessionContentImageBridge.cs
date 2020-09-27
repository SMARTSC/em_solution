namespace EM.Cloud.Persistence.Entities
{
    public class PhotoSessionContentImageBridge
    {
        public System.Guid PhotoSessionId { get; set; }

        public PhotoSessionContent PhotoSession { get; set; }

        public System.Guid ImageId { get; set; }

        public SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image Image { get; set; }
    }
}
