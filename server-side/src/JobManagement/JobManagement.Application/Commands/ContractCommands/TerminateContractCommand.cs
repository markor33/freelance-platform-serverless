using FluentResults;
using MediatR;
using System.Text.Json.Serialization;

namespace JobManagement.Application.Commands.ContractCommands
{
    public class TerminateContractCommand : IRequest<Result>
    {
        public Guid JobId { get; set; }
        public Guid ContractId { get; private set; }

        [JsonConstructor]
        public TerminateContractCommand(Guid jobId, Guid contractId)
        {
            JobId = jobId;
            ContractId = contractId;
        }

    }
}
