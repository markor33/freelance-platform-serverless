namespace ReadModel;

public interface IJobReadModelRepository
{
    Task<JobViewModel> GetByIdAsync(Guid id);
    Task<List<JobViewModel>> GetByIdsAsync(HashSet<Guid> ids);
    Task<List<JobViewModel>> GetByClient(Guid clientId);
    Task<List<JobViewModel>> GetAllAsync();
    Task SaveAsync(JobViewModel jobViewModel);
}
