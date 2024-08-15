using FluentValidation;
using JobManagement.Application.Commands.ProposalCommands;

namespace JobManagement.Application.Validations
{
    public class CreateProposalCommandValidator : AbstractValidator<CreateProposalCommand>
    {
        public CreateProposalCommandValidator()
        {
            RuleFor(x => x.JobId).NotEmpty();

            RuleFor(x => x.FreelancerId).NotEmpty();

            RuleFor(x => x.Text).NotEmpty();

            RuleFor(x => x.Payment).NotEmpty();
        }
    }
}
