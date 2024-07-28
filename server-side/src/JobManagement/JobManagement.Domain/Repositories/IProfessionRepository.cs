using JobManagement.Domain.AggregatesModel.JobAggregate.Entities;

namespace JobManagement.Domain.Repositories
{
    public interface IProfessionRepository
    {
        Task<Profession> GetByIdAsync(Guid id);
    }

}
