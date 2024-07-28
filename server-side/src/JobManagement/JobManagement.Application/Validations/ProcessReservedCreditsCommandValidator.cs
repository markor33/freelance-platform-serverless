using FluentValidation;
using JobManagement.Application.Commands.ProposalCommands;

namespace JobManagement.Application.Validations
{
    public class ProcessReservedCreditsCommandValidator : AbstractValidator<ProcessReservedCreditsCommand>
    {
        public ProcessReservedCreditsCommandValidator()
        {
            RuleFor(x => x.JobId).NotEmpty();

            RuleFor(x => x.ProposalId).NotEmpty();
        }
    }
}
