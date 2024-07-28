namespace JobManagement.Application.Queries
{
    public interface IContractQueries
    {

        Task<List<ContractViewModel>> GetByClient(Guid clientId);
        Task<List<ContractViewModel>> GetByFreelancer(Guid freelancerId);
        Task<List<ContractViewModel>> GetByJob(Guid jobId);
    }
}
