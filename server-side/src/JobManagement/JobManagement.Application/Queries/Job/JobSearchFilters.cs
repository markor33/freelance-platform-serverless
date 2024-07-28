using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;

namespace JobManagement.Application.Queries
{
    public class JobSearchFilters
    {
        public List<Guid> Professions { get; set; } = new List<Guid>();
        public List<ExperienceLevel> ExperienceLevels { get; set; } = new List<ExperienceLevel>();
        public List<PaymentType> PaymentTypes { get; set; } = new List<PaymentType>();

        public string ApplyFilters(string query)
        {
            if (Professions.Count > 0)
            {
                query += @" AND j.""ProfessionId"" = ANY(@Professions)";
            }
            if (ExperienceLevels.Count > 0)
            {
                query += @" AND j.""ExperienceLevel"" = ANY(@ExperienceLevels)";
            }
            if (PaymentTypes.Count > 0)
            {
                query += @" AND j.""Payment_Type"" = ANY(@PaymentTypes)";
            }

            return query;
        }
    }
}
