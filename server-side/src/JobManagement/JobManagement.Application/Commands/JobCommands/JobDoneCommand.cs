using FluentResults;
using MediatR;
using System.Text.Json.Serialization;

namespace JobManagement.Application.Commands.JobCommands
{
    public class JobDoneCommand : IRequest<Result>
    {
        public Guid JobId { get; private set; }

        [JsonConstructor]
        public JobDoneCommand(Guid jobId)
        {
            JobId = jobId;
        }
    }
}
