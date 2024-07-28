using FluentValidation;
using JobManagement.Application.Commands.JobCommands;

namespace JobManagement.Application.Validations
{
    public class UpdateJobCommandValidator : AbstractValidator<UpdateJobCommand>
    {
        public UpdateJobCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty();

            RuleFor(x => x.Description).NotEmpty();

            RuleFor(x => x.ExperienceLevel).IsInEnum();

            RuleFor(x => x.Payment).NotEmpty();

            RuleFor(x => x.ProfessionId).NotEmpty();
        }
    }
}
