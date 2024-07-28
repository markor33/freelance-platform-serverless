using FluentValidation;
using JobManagement.Application.Commands.ContractCommands;

namespace JobManagement.Application.Validations
{
    public class FreelancerAcceptProposalCommandValidator : AbstractValidator<MakeContractCommand>
    {
        public FreelancerAcceptProposalCommandValidator()
        {
            RuleFor(x => x.JobId);

            RuleFor(x => x.ProposalId);
        }
    }
}
