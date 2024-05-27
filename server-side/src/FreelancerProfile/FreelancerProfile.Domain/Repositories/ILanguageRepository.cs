using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;

namespace FreelancerProfile.Domain.Repositories
{
    public interface ILanguageRepository
    {
        Task<Language> GetByIdAsync(int id);
    }
}
