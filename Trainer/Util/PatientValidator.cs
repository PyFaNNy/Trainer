using FluentValidation;
using Trainer.Models;

namespace Trainer.Util
{
    public class PatientValidator : AbstractValidator<PatientViewModel>
    {
        public PatientValidator()
        {
            RuleFor(peopleDTO => peopleDTO.FirstName).NotNull().Matches(@"^([А-Я]{1}[а-яё]{1,49}|[A-Z]{1}[a-z]{1,49})$");
            RuleFor(peopleDTO => peopleDTO.LastName).NotNull().Matches(@"^([А-Я]{1}[а-яё]{1,49}|[A-Z]{1}[a-z]{1,49})$");
            RuleFor(peopleDTO => peopleDTO.MiddleName).NotNull().Matches(@"^([А-Я]{1}[а-яё]{1,49}|[A-Z]{1}[a-z]{1,49})$");
            RuleFor(peopleDTO => peopleDTO.Email).NotNull().Matches(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            RuleFor(peopleDTO => peopleDTO.Age).ExclusiveBetween(1, 110);
        }
    }
}
