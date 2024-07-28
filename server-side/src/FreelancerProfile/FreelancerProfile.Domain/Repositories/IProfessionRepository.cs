using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;

namespace FreelancerProfile.Domain.Repositories
{
    public interface IProfessionRepository
    {
        Task<List<Profession>> Get();
        Task<Profession> GetByIdAsync(Guid id);
        Task<List<Skill>> GetSkills(Guid id);
    }

}
