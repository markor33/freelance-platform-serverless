using System.Data;
using Dapper;


namespace JobManagement.Application.Queries
{
    public class AnswerQueries : IAnswerQueries
    {
        private readonly IDbConnection _dbConnection;

        public AnswerQueries(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<AnswerViewModel>> GetByProposalAsync(Guid proposalId)
        {
            var answers = await _dbConnection.QueryAsync<AnswerViewModel, QuestionViewModel, AnswerViewModel>(
                @"SELECT a.""Id"", a.""Text"",
                        q.""Id"", q.""Text"" 
                    FROM ""Answers"" a
                    INNER JOIN ""Questions"" q ON a.""QuestionId"" = q.""Id""
                    WHERE a.""ProposalId""=@proposalId",
                (answer, question) =>
                {
                    answer.Question = question;
                    return answer;
                },
                new { proposalId });

            return answers.ToList();
        }
    }
}
