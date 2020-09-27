using SmartSolucionesCuba.SAPRESSC.Core.Web.Common.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace EM.Cloud.Models.View
{
    public class OrganizationClientInputViewModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.OrganizationClient, System.Guid>
    {
        public OrganizationClientInputViewModel()
        {
            BornDate = System.DateTime.Now.Date;
        }

        public System.Guid Id { get; set; }

        [FormFileContentTypes(ContentTypes = new string[] { "image/png", "image/jpeg" }, ErrorMessage = "PNG_JPEG_FileAllowedOnlyMessage")]
        [FormFileMaxLength(Length = 3145728, ErrorMessage = "FileLimitSizeExcededMessage")]
        [Display(Name = "ImageFile")]
        public Microsoft.AspNetCore.Http.IFormFile ImageFile { get; set; }

        public System.Guid ImageId { get; set; }

        public bool ResetImage { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "BornDate")]
        public System.DateTime BornDate { get; set; }

        [Display(Name = "About")]
        public string About { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        public Persistence.Entities.OrganizationClient Export()
        {
            var entity = new Persistence.Entities.OrganizationClient();

            Mergue(entity);

            return entity;
        }

        public void Import(Persistence.Entities.OrganizationClient entity)
        {
            Id = entity.Id;
            ImageId = entity.Identity.Image != null ? entity.Identity.Image.Id : System.Guid.Empty;
            Name = entity.Identity.Name;
            BornDate = entity.Identity.BornDate;
            About = entity.Identity.About;
            Notes = entity.Notes;
        }

        public void Mergue(Persistence.Entities.OrganizationClient entity)
        {
            entity.Identity.Name = Name;
            entity.Identity.BornDate = BornDate;
            entity.Identity.About = About;
            entity.Notes = Notes;
        }
    }
}
