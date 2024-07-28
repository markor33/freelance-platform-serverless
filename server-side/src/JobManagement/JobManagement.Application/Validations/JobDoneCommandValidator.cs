using FluentValidation;
using JobManagement.Application.Commands.JobCommands;

namespace JobManagement.Application.Validations
{
    public class JobDoneCommandValidator : AbstractValidator<JobDoneCommand>
    {
        public JobDoneCommandValidator()
        {
            RuleFor(x => x.JobId).NotEmpty();
        }
    }
}
