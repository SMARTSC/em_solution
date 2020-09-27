namespace EM.Cloud.Persistence.Entities
{
    public class PhotoSessionContent : Content
    {
        public PhotoSessionContent()
        {
            ImageBridges = new System.Collections.Generic.List<PhotoSessionContentImageBridge>();
        }

        public System.Collections.Generic.List<PhotoSessionContentImageBridge> ImageBridges { get; set; }
    }
}
