using FluentValidation;
using JobManagement.Application.Commands.ProposalCommands;

namespace JobManagement.Application.Validations
{
    public class DeleteProposalCommandValidator : AbstractValidator<DeleteProposalCommand>
    {
        public DeleteProposalCommandValidator()
        {
            RuleFor(x => x.JobId).NotEmpty();

            RuleFor(x => x.ProposalId).NotEmpty();
        }
    }
}
