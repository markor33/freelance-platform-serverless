using JobSearch.IndexModel;
using System.Text.Json;

namespace JobSearch.Persistence;

public class JobSearchParams
{
    public string FullTextSearch { get; set; } = string.Empty;
    public List<Guid> Professions { get; set; } = [];
    public List<ExperienceLevel> ExperienceLevels { get; set; } = [];
    public List<PaymentType> PaymentTypes { get; set; } = [];
}

public static class JobSearchParamsExtensions
{
    private static readonly string[] item = ["Title", "Description"];

    public static string BuildSearchQuery(this JobSearchParams jobSearchParams)
    {
        var query = new
        {
            query = new
            {
                @bool = new
                {
                    must = new List<object>()
                }
            }
        };

        query.query.@bool.must.Add(new
        {
            terms = new
            {
                Status = new List<int>() { 0, 1 }
            }
        });

        if (!string.IsNullOrEmpty(jobSearchParams.FullTextSearch))
        {
            query.query.@bool.must.Add(new
            {
                multi_match = new
                {
                    query = jobSearchParams.FullTextSearch,
                    fields = item
                }
            });
        }

        if (jobSearchParams.Professions.Any())
        {
            query.query.@bool.must.Add(new
            {
                terms = new
                {
                    ProfessionId = jobSearchParams.Professions
                }
            });
        }

        if (jobSearchParams.ExperienceLevels.Any())
        {
            query.query.@bool.must.Add(new
            {
                terms = new
                {
                    ExperienceLevel = jobSearchParams.ExperienceLevels
                }
            });
        }

        if (jobSearchParams.PaymentTypes.Any())
        {
            query.query.@bool.must.Add(new
            {
                terms = new Dictionary<string, object>
                {
                    { "Payment.Type", jobSearchParams.PaymentTypes }
                }
            });
        }

        return JsonSerializer.Serialize(query);
    }
}