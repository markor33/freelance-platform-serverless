using FluentValidation;
using JobManagement.Application.Commands.JobCommands;

namespace JobManagement.Application.Validations
{
    public class DeleteJobCommandValidator : AbstractValidator<DeleteJobCommand>
    {
        public DeleteJobCommandValidator()
        {
            RuleFor(x => x.JobId).NotEmpty();
        }
    }
}
