using FluentValidation;
using JobManagement.Application.Commands.ProposalCommands;

namespace JobManagement.Application.Validations
{
    public class ProcessInitialMessageSentCommandValidator : AbstractValidator<ProcessInitialMessageSentCommand>
    {
        public ProcessInitialMessageSentCommandValidator()
        {
            RuleFor(x => x.JobId).NotEmpty();

            RuleFor(x => x.ProposalId).NotEmpty();
            
            RuleFor(x => x.FreelancerId).NotEmpty();
        }
    }
}
