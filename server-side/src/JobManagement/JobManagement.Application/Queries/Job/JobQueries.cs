using Dapper;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;
using System.Data;

namespace JobManagement.Application.Queries
{
    public class JobQueries : IJobQueries
    {
        private readonly IDbConnection _dbConnection;

        public JobQueries(
            IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<JobViewModel>> Search(string queryText, JobSearchFilters filters)
        {
            if (queryText is null) queryText = "";
            var searchQuery = @"SELECT j.""Id"", j.""ClientId"", j.""Title"", j.""Description"", j.""Created"", j.""ExperienceLevel"", j.""Credits"", j.""Status"",
                        (SELECT COUNT(*) FROM ""Proposals"" pr WHERE pr.""JobId"" = j.""Id"") AS NumOfProposals,
                        (SELECT COUNT(*) FROM ""Proposals"" pr WHERE pr.""JobId"" = j.""Id"" AND (pr.""Status"" = 1 OR pr.""Status"" = 2)) AS CurrentlyInterviewing,
                        j.""Payment_Amount"" as Amount, j.""Payment_Currency"" as Currency, j.""Payment_Type"" as Type
                    FROM ""Jobs"" j 
                    LEFT JOIN ""Proposals"" pr on pr.""JobId"" = j.""Id""
                    WHERE j.""Status"" != 3 AND (LOWER(j.""Title"") LIKE @queryText OR LOWER(j.""Description"") LIKE @queryText)";
            
            if (filters is not null)
                searchQuery = filters.ApplyFilters(searchQuery);
            searchQuery += @" GROUP BY j.""Id"" ORDER BY j.""Created"" DESC";

            var jobs = await _dbConnection.QueryAsync<JobViewModel, Payment, JobViewModel>(
                searchQuery,
                (job, payment) =>
                {
                    job.Payment = payment;
                    return job;
                },
                new { 
                    queryText = $"%{queryText.ToLower()}%", 
                    filters.Professions, 
                    ExperienceLevels = filters.ExperienceLevels.Select(x => (int)x).ToList(),
                    PaymentTypes = filters.PaymentTypes.Select(x => (int)x).ToList() },
                splitOn: "Amount");

            return await GroupJobs(jobs);
        }

        public async Task<List<JobViewModel>> GetAllAsync()
        {
            var jobs = await _dbConnection.QueryAsync<JobViewModel, Payment, ProfessionViewModel, QuestionViewModel, SkillViewModel, JobViewModel>(
                @"SELECT j.""Id"", j.""ClientId"", j.""Title"", j.""Description"", j.""Created"", j.""ExperienceLevel"", j.""Credits"", j.""Status"",
                        (SELECT COUNT(*) FROM ""Proposals"" pr WHERE pr.""JobId"" = j.""Id"") AS NumOfProposals,
                        (SELECT COUNT(*) FROM ""Proposals"" pr WHERE pr.""JobId"" = j.""Id"" AND (pr.""Status"" = 1 OR pr.""Status"" = 2)) AS CurrentlyInterviewing,
                        (SELECT COUNT(*) FROM ""Contracts"" c WHERE c.""JobId"" = j.""Id"" AND c.""Status"" = 0) AS NumOfActiveContracts,
                        (SELECT COUNT(*) FROM ""Contracts"" c WHERE c.""JobId"" = j.""Id"" AND c.""Status"" = 1) AS NumOfFinishedContracts,
                        j.""Payment_Amount"" as Amount, j.""Payment_Currency"" as Currency, j.""Payment_Type"" as Type,
                        p.""Id"", p.""Name"", p.""Description"",
                        q.""Id"", q.""Text"",
                        s.""Id"", s.""Name"", s.""Description""
                    FROM ""Jobs"" j 
                    INNER JOIN ""Professions"" p ON j.""ProfessionId"" = p.""Id""
                    LEFT JOIN ""Proposals"" pr on pr.""JobId"" = j.""Id""
                    LEFT JOIN ""Contracts"" c ON c.""JobId"" = j.""Id""
                    LEFT JOIN ""Questions"" q ON j.""Id"" = q.""JobId""
                    LEFT JOIN ""JobSkill"" js ON j.""Id"" = js.""JobsId""
                    LEFT JOIN ""Skills"" s ON s.""Id"" = js.""SkillsId""
                    WHERE j.""Status"" != 3
                    GROUP BY j.""Id"", p.""Id"", q.""Id"", s.""Id""",
                (job, payment, profession, question, skill) =>
                {
                    job.Profession = profession;
                    job.Payment = payment;
                    job.Questions.Add(question);
                    job.Skills.Add(skill);
                    return job;
                },
                splitOn: "Amount, Id, Id, Id");

            return await GroupJobs(jobs);
        }

        public async Task<JobViewModel> GetByIdAsync(Guid id)
        {
            var jobs = await _dbConnection.QueryAsync<JobViewModel, Payment, ProfessionViewModel, QuestionViewModel, SkillViewModel, JobViewModel>(
                @"SELECT j.""Id"", j.""ClientId"", j.""Title"", j.""Description"", j.""Created"", j.""ExperienceLevel"", j.""Credits"", j.""Status"",
                        (SELECT COUNT(*) FROM ""Proposals"" pr WHERE pr.""JobId"" = j.""Id"") AS NumOfProposals,
                        (SELECT COUNT(*) FROM ""Proposals"" pr WHERE pr.""JobId"" = j.""Id"" AND (pr.""Status"" = 1 OR pr.""Status"" = 2)) AS CurrentlyInterviewing,
                        (SELECT COUNT(*) FROM ""Contracts"" c WHERE c.""JobId"" = j.""Id"" AND c.""Status"" = 0) AS NumOfActiveContracts,
                        (SELECT COUNT(*) FROM ""Contracts"" c WHERE c.""JobId"" = j.""Id"" AND c.""Status"" = 1) AS NumOfFinishedContracts,
                        j.""Payment_Amount"" as Amount, j.""Payment_Currency"" as Currency, j.""Payment_Type"" as Type,
                        p.""Id"", p.""Name"", p.""Description"",
                        q.""Id"", q.""Text"",
                        s.""Id"", s.""Name"", s.""Description""
                    FROM ""Jobs"" j 
                    INNER JOIN ""Professions"" p ON j.""ProfessionId"" = p.""Id""
                    LEFT JOIN ""Proposals"" pr on pr.""JobId"" = j.""Id""
                    LEFT JOIN ""Contracts"" c ON c.""JobId"" = j.""Id""
                    LEFT JOIN ""Questions"" q ON j.""Id"" = q.""JobId""
                    LEFT JOIN ""JobSkill"" js ON j.""Id"" = js.""JobsId""
                    LEFT JOIN ""Skills"" s ON s.""Id"" = js.""SkillsId""
                    WHERE j.""Status"" != 3 AND j.""Id""=@id
                    GROUP BY j.""Id"", p.""Id"", q.""Id"", s.""Id""",
                (job, payment, profession, question, skill) =>
                {
                    job.Profession = profession;
                    job.Payment = payment;
                    job.Questions.Add(question);
                    job.Skills.Add(skill);
                    return job;
                },
                new { id },
                splitOn: "Amount, Id, Id, Id");

            return (await GroupJobs(jobs)).First();
        }

        public async Task<List<JobViewModel>> GetByClientAsync(Guid clientId)
        {
            var jobs = await _dbConnection.QueryAsync<JobViewModel, Payment, ProfessionViewModel, QuestionViewModel, SkillViewModel, JobViewModel>(
                @"SELECT j.""Id"", j.""ClientId"", j.""Title"", j.""Description"", j.""Created"", j.""ExperienceLevel"", j.""Credits"", j.""Status"",
                        (SELECT COUNT(*) FROM ""Proposals"" pr WHERE pr.""JobId"" = j.""Id"") AS NumOfProposals,
                        (SELECT COUNT(*) FROM ""Proposals"" pr WHERE pr.""JobId"" = j.""Id"" AND (pr.""Status"" = 1 OR pr.""Status"" = 2)) AS CurrentlyInterviewing,
                        (SELECT COUNT(*) FROM ""Contracts"" c WHERE c.""JobId"" = j.""Id"" AND c.""Status"" = 0) AS NumOfActiveContracts,
                        (SELECT COUNT(*) FROM ""Contracts"" c WHERE c.""JobId"" = j.""Id"" AND c.""Status"" = 1) AS NumOfFinishedContracts,
                        j.""Payment_Amount"" as Amount, j.""Payment_Currency"" as Currency, j.""Payment_Type"" as Type,
                        p.""Id"", p.""Name"", p.""Description"",
                        q.""Id"", q.""Text"",
                        s.""Id"", s.""Name"", s.""Description""
                    FROM ""Jobs"" j 
                    INNER JOIN ""Professions"" p ON j.""ProfessionId"" = p.""Id""
                    LEFT JOIN ""Proposals"" pr on pr.""JobId"" = j.""Id""
                    LEFT JOIN ""Contracts"" c ON c.""JobId"" = j.""Id""
                    LEFT JOIN ""Questions"" q ON j.""Id"" = q.""JobId""
                    LEFT JOIN ""JobSkill"" js ON j.""Id"" = js.""JobsId""
                    LEFT JOIN ""Skills"" s ON s.""Id"" = js.""SkillsId""
                    WHERE j.""Status"" != 3 AND j.""ClientId""=@clientId
                    GROUP BY j.""Id"", p.""Id"", q.""Id"", s.""Id""",
                (job, payment, profession, question, skill) =>
                {
                    job.Profession = profession;
                    job.Payment = payment;
                    job.Questions.Add(question);
                    job.Skills.Add(skill);
                    return job;
                },
                new { clientId },
                splitOn: "Amount, Id, Id, Id");

            return await GroupJobs(jobs);
        }

        private static async Task<List<JobViewModel>> GroupJobs(IEnumerable<JobViewModel> jobs)
        {
            var groupedJobs = jobs.GroupBy(
                job => job.Id,
                (key, group) => new JobViewModel(
                    key,
                    group.Select(job => job.ClientId).FirstOrDefault(),
                    group.Select(job => job.Title).FirstOrDefault(),
                    group.Select(job => job.Description).FirstOrDefault(),
                    group.Select(job => job.Created).FirstOrDefault(),
                    group.Select(job => job.ExperienceLevel).FirstOrDefault(),
                    group.Select(job => job.Status).FirstOrDefault(),
                    group.Select(job => job.Payment).FirstOrDefault(),
                    group.Select(job => job.Credits).FirstOrDefault(),
                    group.SelectMany(job => job.Questions).Distinct().ToList(),
                    group.Select(job => job.Profession).FirstOrDefault(),
                    group.SelectMany(job => job.Skills).Distinct().ToList(),
                    group.Select(job => job.NumOfProposals).FirstOrDefault(),
                    group.Select(job => job.CurrentlyInterviewing).FirstOrDefault(),
                    group.Select(job => job.NumOfActiveContracts).FirstOrDefault(),
                    group.Select(job => job.NumOfFinishedContracts).FirstOrDefault())).ToList();

            return groupedJobs;
        }
    }
}
