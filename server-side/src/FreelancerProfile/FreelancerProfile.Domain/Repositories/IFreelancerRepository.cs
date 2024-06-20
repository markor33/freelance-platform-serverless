using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate;
using FreelancerProfile.Domain.SeedWork;

namespace FreelancerProfile.Domain.Repositories
{
    public interface IFreelancerRepository : IRepository<Freelancer>
    {
        Task<Freelancer> GetByIdAsync(Guid id);
        Task SaveAsync(Freelancer freelancer);
    }
}
