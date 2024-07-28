using FluentResults;
using MediatR;

namespace JobManagement.Application.Commands.ContractCommands
{
    public class FinishContractCommand : IRequest<Result>
    {
        public Guid JobId { get; private set; }
        public Guid ContractId { get; private set; }

        public FinishContractCommand(Guid jobId, Guid contractId)
        {
            JobId = jobId;
            ContractId = contractId;
        }
    }
}
