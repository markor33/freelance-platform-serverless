using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcJobManagement;
using JobManagement.Application.Queries;

namespace JobManagement.API.GrpcServices
{
    public class ContractGrpcService : Contract.ContractBase
    {
        private readonly IContractQueries _contractQueries;

        public ContractGrpcService(IContractQueries contractQueries)
        {
            _contractQueries = contractQueries;
        }

        public override async Task<ContractsListResponse> GetContractsByClient(GetContractsByClientRequest request, ServerCallContext context)
        {
            var contracts = await _contractQueries.GetByClient(Guid.Parse(request.ClientId));
            return CreateContractsListResponse(contracts);
        }

        public override async Task<ContractsListResponse> GetContractsByJob(GetContractsByJobRequest request, ServerCallContext context)
        {
            var contracts = await _contractQueries.GetByJob(Guid.Parse(request.JobId));
            return CreateContractsListResponse(contracts);
        }

        private static ContractsListResponse CreateContractsListResponse(List<ContractViewModel> contracts)
        {
            var response = new ContractsListResponse();
            foreach (var contract in contracts)
            {
                var dto = new ContractDTO()
                {
                    Id = contract.Id.ToString(),
                    JobId = contract.JobId.ToString(),
                    JobTitle = contract.JobTitle,
                    ClientId = contract.ClientId.ToString(),
                    FreelancerId = contract.FreelancerId.ToString(),
                    Payment = new Payment()
                    {
                        Amount = contract.Payment.Amount,
                        Currency = contract.Payment.Currency,
                        Type = (int)contract.Payment.Type
                    },
                    Started = Timestamp.FromDateTime(contract.Started),
                    Status = (int)contract.Status
                };
                if (contract.Finished is not null) dto.Finished = Timestamp.FromDateTime((DateTime)contract.Finished);
                response.Contracts.Add(dto);
            }

            return response;
        }

    }
}
