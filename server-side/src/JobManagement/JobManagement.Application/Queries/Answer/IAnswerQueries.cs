namespace JobManagement.Application.Queries
{
    public interface IAnswerQueries
    {
        Task<List<AnswerViewModel>> GetByProposalAsync(Guid proposalId);
    }
}
