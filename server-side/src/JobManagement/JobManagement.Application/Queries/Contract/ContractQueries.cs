using Dapper;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;
using System.Data;

namespace JobManagement.Application.Queries
{
    public class ContractQueries : IContractQueries
    {
        private readonly IDbConnection _dbConnection;

        public ContractQueries(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<ContractViewModel>> GetByClient(Guid clientId)
        {
            var contracts = await _dbConnection.QueryAsync<ContractViewModel, Payment, ContractViewModel>(
                @"SELECT c.""Id"", c.""ClientId"", c.""FreelancerId"", c.""Started"", c.""Finished"", c.""Status"",
                            j.""Id"" as JobId, j.""Title"" as JobTitle,
                            c.""Payment_Amount"" as Amount, c.""Payment_Currency"" as Currency, c.""Payment_Type"" as Type
                    FROM ""Contracts"" c
                    INNER JOIN ""Jobs"" j ON c.""JobId"" = j.""Id""
                    WHERE c.""ClientId"" = @clientId
                ",
                (contract, payment) =>
                {
                    contract.Payment = payment;
                    return contract;
                },
                new { clientId },
                splitOn: "Amount");

            return contracts.ToList();
        }

        public async Task<List<ContractViewModel>> GetByFreelancer(Guid freelancerId)
        {
            var contracts = await _dbConnection.QueryAsync<ContractViewModel, Payment, ContractViewModel>(
                @"SELECT c.""Id"", c.""ClientId"", c.""FreelancerId"", c.""Started"", c.""Finished"", c.""Status"",
                            j.""Id"" as JobId, j.""Title"" as JobTitle,
                            c.""Payment_Amount"" as Amount, c.""Payment_Currency"" as Currency, c.""Payment_Type"" as Type
                    FROM ""Contracts"" c
                    INNER JOIN ""Jobs"" j ON c.""JobId"" = j.""Id""
                    WHERE c.""FreelancerId"" = @freelancerId
                ",
                (contract, payment) =>
                {
                    contract.Payment = payment;
                    return contract;
                },
                new { freelancerId },
                splitOn: "Amount");

            return contracts.ToList();
        }

        public async Task<List<ContractViewModel>> GetByJob(Guid jobId)
        {
            var contracts = await _dbConnection.QueryAsync<ContractViewModel, Payment, ContractViewModel>(
                @"SELECT c.""Id"", c.""ClientId"", c.""FreelancerId"", c.""Started"", c.""Finished"", c.""Status"",
                            j.""Id"" as JobId, j.""Title"" as JobTitle,
                            c.""Payment_Amount"" as Amount, c.""Payment_Currency"" as Currency, c.""Payment_Type"" as Type
                    FROM ""Contracts"" c
                    INNER JOIN ""Jobs"" j ON c.""JobId"" = j.""Id""
                    WHERE c.""JobId"" = @jobId
                ",
                (contract, payment) => 
                {
                    contract.Payment = payment;
                    return contract;
                },
                new { jobId },
                splitOn: "Amount");

            return contracts.ToList();
        }

    }
}
