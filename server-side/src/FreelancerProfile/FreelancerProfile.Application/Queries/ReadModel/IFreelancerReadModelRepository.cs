namespace ReadModel;

public interface IFreelancerReadModelRepository
{
    Task<FreelancerViewModel> GetByIdAsync(Guid id);
    Task SaveAsync(FreelancerViewModel freelancer);
}
