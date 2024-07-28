using FluentValidation;
using JobManagement.Application.Commands.ProposalCommands;

namespace JobManagement.Application.Validations
{
    public class ClientAcceptProposalCommandValidator : AbstractValidator<ApproveProposalCommand>
    {
        public ClientAcceptProposalCommandValidator()
        {
            RuleFor(x => x.JobId).NotEmpty();

            RuleFor(x => x.ProposalId).NotEmpty();
        }
    }
}
