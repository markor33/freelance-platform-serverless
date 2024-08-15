using ReadModel;

namespace JobManagement.ReadModel;

public interface IContractReadModelRepository
{
    Task<ContractViewModel> GetById(Guid id, Guid jobId);
    Task<List<ContractViewModel>> GetByJob(Guid jobId);
    Task<List<ContractViewModel>> GetByClient(Guid clientId);
    Task<List<ContractViewModel>> GetByFreelancer(Guid freelancerId);
    Task SaveAsync(ContractViewModel model);
}
