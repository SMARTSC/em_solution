using SmartSolucionesCuba.SAPRESSC.Core.Web.Common.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace EM.Cloud.Models.View
{
    public class EventInputViewModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.Event, System.Guid>
    {
        public EventInputViewModel()
        {
            Start = System.DateTime.Now;
            End = System.DateTime.Now.AddDays(3);
        }

        public System.Guid Id { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        public string Name { get; set; }

        [Display(Name = "LogoFile")]
        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [FormFileContentTypes(ContentTypes = new string[] { "image/png", "image/jpeg" }, ErrorMessage = "PNG_JPEG_FileAllowedOnlyMessage")]
        [FormFileMaxLength(Length = 3145728, ErrorMessage = "FileLimitSizeExcededMessage")]
        public Microsoft.AspNetCore.Http.IFormFile LogoFile { get; set; }

        public System.Guid LogoId { get; set; }

        //[Required(ErrorMessage = "RequiredValidationErrorMessage")]
        //[FormFileContentTypes(ContentTypes = new string[] { "image/png", "image/jpeg" }, ErrorMessage = "PNG_JPEG_FileAllowedOnlyMessage")]
        //[Display(Name = "PrintLogoMiddleFile")]
        //public Microsoft.AspNetCore.Http.IFormFile PrintLogoMiddleFile { get; set; }

        //public System.Guid PrintLogoMiddleId { get; set; }

        //[Required(ErrorMessage = "RequiredValidationErrorMessage")]
        //[FormFileContentTypes(ContentTypes = new string[] { "image/png", "image/jpeg" }, ErrorMessage = "PNG_JPEG_FileAllowedOnlyMessage")]
        //[Display(Name = "PrintMiddleStrechLogoFile")]
        //public Microsoft.AspNetCore.Http.IFormFile PrintMiddleStrechLogoFile { get; set; }

        //public System.Guid PrintMiddleStrechLogoId { get; set; }

        //[Required(ErrorMessage = "RequiredValidationErrorMessage")]
        //[FormFileContentTypes(ContentTypes = new string[] { "image/png", "image/jpeg" }, ErrorMessage = "PNG_JPEG_FileAllowedOnlyMessage")]
        //[Display(Name = "PrintLogoSmallFile")]
        //public Microsoft.AspNetCore.Http.IFormFile PrintLogoSmallFile { get; set; }

        //public System.Guid PrintLogoSmallId { get; set; }

        //[Required(ErrorMessage = "RequiredValidationErrorMessage")]
        //[FormFileContentTypes(ContentTypes = new string[] { "image/png", "image/jpeg" }, ErrorMessage = "PNG_JPEG_FileAllowedOnlyMessage")]
        //[Display(Name = "PrintSmallStrechLogoFile")]
        //public Microsoft.AspNetCore.Http.IFormFile PrintSmallStrechLogoFile { get; set; }

        //public System.Guid PrintSmallStrechLogoId { get; set; }

        [MaxLength(255, ErrorMessage = "MaxLengthValidationErrorMessage")]
        public string Descrition { get; set; }

        public System.DateTime Start { get; set; }

        public System.DateTime End { get; set; }

        public string PlaceName { get; set; }

        public string Annotations { get; set; }

        public Options.EventMediaDimensionContraintOptions MediaDimensionContraintOptions { get; set; }

        public Persistence.Entities.Event Export()
        {
            var entity = new Persistence.Entities.Event();

            Mergue(entity);

            return entity;
        }

        public void Import(Persistence.Entities.Event entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            LogoId = entity.Logo != null ? entity.Logo.Id : System.Guid.Empty;
            //PrintLogoMiddleId = entity.PrintMiddleLogo?.Id ?? System.Guid.Empty;
            //PrintMiddleStrechLogoId = entity.PrintMiddleStrechLogo?.Id ?? System.Guid.Empty;
            //PrintLogoSmallId = entity.PrintSmallLogo?.Id ?? System.Guid.Empty;
            //PrintSmallStrechLogoId = entity.PrintSmallStrechLogo?.Id ?? System.Guid.Empty;
            Descrition = entity.Descrition;
            Start = entity.Start;
            End = entity.End;
            PlaceName = entity.PlaceName;
            Annotations = entity.Annotations;
        }

        public void Mergue(Persistence.Entities.Event entity)
        {
            entity.Name = Name;
            entity.Descrition = Descrition;
            entity.Start = Start;
            entity.End = End;
            entity.PlaceName = PlaceName;
            entity.Annotations = Annotations;
        }
    }
}
