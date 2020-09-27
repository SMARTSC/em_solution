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

        public CollageLayout DefaultPictureContentCollageLayout { get; set; }

        public CollageLayout DefaultPhotoSessionContentCollageLayout { get; set; }

        public SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image FrontImage { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public Event Event { get; set; }

        public ICollection<Content> Contents { get; set; }

        public ICollection<AlbumCollageLayoutBridge> CollageLayoutBridges { get; set; }

        public ICollection<AlbumLockBridge> LockBridges { get; set; }
    }
}
