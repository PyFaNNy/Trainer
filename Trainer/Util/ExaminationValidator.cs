using FluentValidation;
using Trainer.Models;

namespace Trainer.Util
{
    public class ExaminationValidator : AbstractValidator<ExaminationViewModel>
    {
        public ExaminationValidator()
        {
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.PatientId).NotNull();
            RuleFor(x => x.TypePhysicalActive).NotNull();
            RuleFor(peopleDTO => peopleDTO.Indicators).ExclusiveBetween(1, 31);
        }
    }
}
