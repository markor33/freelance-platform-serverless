namespace ReadModel;

public interface IFreelancerReadModelRepository
{
    Task<FreelancerViewModel> GetByIdAsync(Guid id);
    Task<List<FreelancerViewModel>> GetByIdsAsync(HashSet<Guid> ids);
    Task SaveAsync(FreelancerViewModel freelancer);
}
