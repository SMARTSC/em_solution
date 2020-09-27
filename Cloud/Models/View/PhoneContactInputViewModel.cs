using SmartSolucionesCuba.SAPRESSC.Middleware.Identity.Persistence.Entities;
using System.ComponentModel.DataAnnotations;

namespace EM.Cloud.Models.View
{
    public class PhoneContactInputViewModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityInputModel<PhoneContactItem, System.Guid>
    {
        public System.Guid Id { get; set; }

        [Display(Name = "Label")]
        public string Label { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Phone(ErrorMessage = "PhoneValidationErrorMessage")]
        [Display(Name = "Number")]
        public string Number { get; set; }

        public System.Guid ClientId { get; set; }

        public PhoneContactItem Export()
        {
            var entity = new PhoneContactItem();

            Mergue(entity);

            return entity;
        }

        public void Import(PhoneContactItem entity)
        {
            Label = entity.Label;
            Number = entity.PhoneNumber;
        }

        public void Mergue(PhoneContactItem entity)
        {
            entity.Label = Label;
            entity.PhoneNumber = Number;
        }
    }
}
