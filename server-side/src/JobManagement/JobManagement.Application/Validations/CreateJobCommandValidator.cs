using FluentValidation;
using JobManagement.Application.Commands.JobCommands;

namespace JobManagement.Application.Validations
{
    public class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
    {
        public CreateJobCommandValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty();

            RuleFor(X => X.Title).NotEmpty();

            RuleFor(X => X.Description).NotEmpty();
        }

    }
}
