using FluentResults;

namespace JobManagement.Application.Queries
{
    public interface IProposalQueries
    {
        Task<List<ProposalViewModel>> GetByJobId(Guid jobId);
        Task<ProposalViewModel> GetByIdAsync(Guid id);
    }
}
