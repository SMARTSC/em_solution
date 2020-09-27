namespace EM.Cloud.Persistence.Entities
{
    public class AnimateContent : Content
    {
        public AnimateContent()
        {
            ImageBridges = new System.Collections.Generic.List<AnimateContentImageBridge>();
        }

        public System.Collections.Generic.List<AnimateContentImageBridge> ImageBridges { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public System.TimeSpan Interpolation { get; set; }
    }
}
