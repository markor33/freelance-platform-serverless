using ReadModel;

namespace JobManagement.ReadModel;

public interface IProposalReadModelRepository
{
    Task<ProposalViewModel> GetById(Guid id);
    Task<List<ProposalViewModel>> GetByJob(Guid jobId);
    Task SaveAsync(ProposalViewModel model);
    Task DeleteAsync(Guid proposalId);
}
