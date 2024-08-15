namespace ReadModel;

public interface IFreelancerReadModelRepository
{
    Task<FreelancerViewModel> GetByIdAsync(Guid id);
    Task<List<FreelancerViewModel>> GetByIds(List<Guid> ids);
    Task<List<FreelancerViewModel>> GetByIdsAsync(HashSet<Guid> ids);
    Task SaveAsync(FreelancerViewModel freelancer);
}
