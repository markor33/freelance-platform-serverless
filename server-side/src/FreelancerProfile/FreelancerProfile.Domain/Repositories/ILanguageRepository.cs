using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;

namespace FreelancerProfile.Domain.Repositories
{
    public interface ILanguageRepository
    {
        Task<List<Language>> Get();
        Task<Language> GetByIdAsync(int id);
    }
}
