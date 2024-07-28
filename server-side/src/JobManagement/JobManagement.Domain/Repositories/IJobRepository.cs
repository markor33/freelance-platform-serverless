using JobManagement.Domain.AggregatesModel.JobAggregate;

namespace JobManagement.Domain.Repositories
{
    public interface IJobRepository
    {
        Task<Job> GetByIdAsync(Guid id);
        Task<Job> SaveAsync(Job job);
    }
}
