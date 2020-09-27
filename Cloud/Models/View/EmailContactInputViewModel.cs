using SmartSolucionesCuba.SAPRESSC.Middleware.Identity.Persistence.Entities;
using System.ComponentModel.DataAnnotations;

namespace EM.Cloud.Models.View
{
    public class EmailContactInputViewModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<EmailContactItem, System.Guid>
    {
        public System.Guid Id { get; set; }

        [Display(Name = "Label")]
        public string Label { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [EmailAddress(ErrorMessage = "EmailAddressValidationErrorMessage")]
        [Display(Name = "EmailAddress")]
        public string Address { get; set; }

        public System.Guid ClientId { get; set; }

        public EmailContactItem Export()
        {
            var entity = new EmailContactItem();

            Mergue(entity);

            return entity;
        }

        public void Import(EmailContactItem entity)
        {
            Label = entity.Label;
            Address = entity.EmailAddress;
        }

        public void Mergue(EmailContactItem entity)
        {
            entity.Label = Label;
            entity.EmailAddress = Address;
        }
    }
}
