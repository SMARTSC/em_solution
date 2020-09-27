namespace EM.Cloud.Persistence.Entities
{
    public class AnimateContentImageBridge
    {
        public System.Guid AnimateId { get; set; }

        public AnimateContent Animate { get; set; }

        public System.Guid ImageId { get; set; }

        public SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image Image { get; set; }
    }
}
