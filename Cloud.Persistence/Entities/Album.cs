using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EM.Cloud.Persistence.Entities
{
    public class Album : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Entities.IHabilitableEntity<System.Guid>
    {
        public Album()
        {
            Id = System.Guid.NewGuid();
            Contents = new List<Content>();
            CollageLayoutBridges = new List<AlbumCollageLayoutBridge>();
            LockBridges = new List<AlbumLockBridge>();
        }

        [Key]
        public System.Guid Id { get; set; }

        public bool Enabled { get; set; }

        public bool Public { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Alias { get; set; }

        [Required]
        public int PhotoSessionCaptureCount { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("DefaultPictureContentCollageLayoutId")]
        public CollageLayout DefaultPictureContentCollageLayout { get; set; }

        public System.Guid DefaultPictureContentCollageLayoutId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("DefaultPhotoSessionContentCollageLayoutId")]
        public CollageLayout DefaultPhotoSessionContentCollageLayout { get; set; }

        public System.Guid DefaultPhotoSessionContentCollageLayoutId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("OverlayId")]
        public SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image Overlay { get; set; }

        public System.Guid OverlayId { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("EventId")]
        public Event Event { get; set; }

        public System.Guid EventId { get; set; }

        public ICollection<Content> Contents { get; set; }

        public ICollection<AlbumCollageLayoutBridge> CollageLayoutBridges { get; set; }

        public ICollection<AlbumLockBridge> LockBridges { get; set; }
    }
}
