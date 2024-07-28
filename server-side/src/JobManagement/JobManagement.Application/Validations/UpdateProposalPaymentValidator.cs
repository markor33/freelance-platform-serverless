using FluentValidation;
using JobManagement.Application.Commands.ProposalCommands;

namespace JobManagement.Application.Validations
{
    public class UpdateProposalPaymentValidator : AbstractValidator<UpdateProposalPaymentCommand>
    {
        public UpdateProposalPaymentValidator()
        {
            RuleFor(x => x.JobId).NotEmpty();

            RuleFor(x => x.ProposalId).NotEmpty();

            RuleFor(x => x.Payment).NotEmpty();
        }
    }
}
