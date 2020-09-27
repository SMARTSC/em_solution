using EM.Cloud.Persistence.Entities;
using System.ComponentModel.DataAnnotations;

namespace EM.Cloud.Models.View
{
    public class AlbumInputViewModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Album, System.Guid>
    {
        public AlbumInputViewModel()
        {
            AvailableEventSelectListItems = new System.Collections.Generic.List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            SelectableCollageLayouts = new System.Collections.Generic.List<SmartSolucionesCuba.SAPRESSC.Core.Common.Models.View.SelectionDescriptedItemInputViewModel<System.Guid>>();
        }

        public System.Guid Id { get; set; }

        public System.Guid EventId { get; set; }

        public System.Collections.Generic.List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> AvailableEventSelectListItems { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Alias")]
        public string Alias { get; set; }

        [Display(Name = "Enabled")]
        public bool Enabled { get; set; }

        [Display(Name = "Public")]
        public bool Public { get; set; }

        [Required]
        [Display(Name = "PhotoSessionCaptureCount")]
        public int PhotoSessionCaptureCount { get; set; }

        [Required]
        [Display(Name = "DefaultPictureContentCollageLayout")]
        public System.Guid DefaultPictureContentCollageLayoutId { get; set; }

        public Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] DefaultPictureContentCollageLayoutSelectListItems { get; set; }

        [Required]
        [Display(Name = "DefaultPhotoSessionContentCollageLayout")]
        public System.Guid DefaultPhotoSessionContentCollageLayoutId { get; set; }

        public Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] DefaultPhotoSessionContentCollageLayoutSelectListItems { get; set; }

        [Display(Name = "CollageLayouts")]
        public System.Collections.Generic.List<SmartSolucionesCuba.SAPRESSC.Core.Common.Models.View.SelectionDescriptedItemInputViewModel<System.Guid>> SelectableCollageLayouts { get; set; }

        [MaxLength(255)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public Album Export()
        {
            var entity = new Album();

            Mergue(entity);

            return entity;
        }

        public void Import(Album entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            Alias = entity.Alias;
            Enabled = entity.Enabled;
            Public = entity.Public;
            PhotoSessionCaptureCount = entity.PhotoSessionCaptureCount;
            Description = entity.Description;
            DefaultPhotoSessionContentCollageLayoutId = entity.DefaultPhotoSessionContentCollageLayout.Id;
            DefaultPictureContentCollageLayoutId = entity.DefaultPictureContentCollageLayout.Id;
        }

        public void Mergue(Album entity)
        {
            entity.Title = Title;
            entity.Alias = Alias;
            entity.Enabled = Enabled;
            entity.Public = Public;
            entity.PhotoSessionCaptureCount = PhotoSessionCaptureCount;
            entity.Description = Description;
        }
    }
}
