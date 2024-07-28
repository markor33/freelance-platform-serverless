

using Dapper;
using FluentResults;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;
using System.Data;
using System.Linq;

namespace JobManagement.Application.Queries
{
    public class ProposalQueries : IProposalQueries
    {
        private readonly IDbConnection _dbConnection;

        public ProposalQueries(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ProposalViewModel> GetByIdAsync(Guid id)
        {
            var proposals = await _dbConnection.QueryAsync<ProposalViewModel, Payment, AnswerViewModel, QuestionViewModel, ProposalViewModel>(
                @"SELECT p.""Id"", p.""FreelancerId"", p.""Text"", p.""Created"", p.""Status"", 
                            p.""Payment_Amount"" as Amount, p.""Payment_Currency"" as Currency, p.""Payment_Type"" as Type,
                            a.""Id"", a.""Text"",
                            q.""Id"", q.""Text"" 
                    FROM ""Proposals"" p
                    INNER JOIN ""Answers"" a ON a.""ProposalId"" = p.""Id""
                    INNER JOIN ""Questions"" q ON a.""QuestionId"" = q.""Id""
                    WHERE p.""Id""=@id",
                (proposal, payment, answer, question) =>
                {
                    proposal.Payment = payment;
                    answer.Question = question;
                    proposal.Answers.Add(answer);
                    return proposal;
                },
                new { id },
                splitOn: "Amount, Id, Id");

            return GroupProposals(proposals).First();
        }

        public async Task<List<ProposalViewModel>> GetByJobId(Guid jobId)
        {
            var proposals = await _dbConnection.QueryAsync<ProposalViewModel, Payment, ProposalViewModel>(
                @"SELECT ""Id"", ""FreelancerId"", ""Text"", ""Created"", ""Status"", ""Payment_Amount"" as Amount, ""Payment_Currency"" as Currency, ""Payment_Type"" as Type
                    FROM ""Proposals""
                    WHERE ""JobId""=@jobid",
                (proposal, payment) =>
                {
                    proposal.Payment = payment;
                    return proposal;
                },
                new { jobId },
                splitOn: "Amount");

            return proposals.ToList();
        }

        private static List<ProposalViewModel> GroupProposals(IEnumerable<ProposalViewModel> proposals)
        {
            var groupedProposals = proposals.GroupBy(
                proposal => proposal.Id,
                (key, group) => new ProposalViewModel(
                    key,
                    group.Select(proposal => proposal.FreelancerId).FirstOrDefault(),
                    group.Select(proposal => proposal.Text).FirstOrDefault(),
                    group.Select(proposal => proposal.Payment).FirstOrDefault(),
                    group.Select(proposal => proposal.Status).FirstOrDefault(),
                    group.Select(proposal => proposal.Created).FirstOrDefault(),
                    group.SelectMany(proposal => proposal.Answers).Distinct().ToList()));

            return groupedProposals.ToList();
        }

    }
}
