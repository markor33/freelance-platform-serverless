using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;

namespace FreelancerProfile.Domain.Repositories
{
    public interface ISkillRepository
    {
        Task<Skill> GetByIdAsync(Guid id);
        Task<List<Skill>> GetByIdsAsync(List<Guid> ids);
    }
}
