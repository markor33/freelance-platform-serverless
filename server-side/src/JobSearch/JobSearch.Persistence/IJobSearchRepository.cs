using JobSearch.IndexModel;

namespace JobSearch.Persistence;

public interface IJobSearchRepository
{
    Task<Job> GetById(Guid id);
    Task<List<Job>> Search(JobSearchParams jobSearchParams);
    Task Index(Job job);
    Task Update(Job job);
}
