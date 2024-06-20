using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;

namespace FreelancerProfile.Domain.Repositories
{
    public interface IProfessionRepository
    {
        Task<Profession> GetByIdAsync(Guid id);
    }

}
