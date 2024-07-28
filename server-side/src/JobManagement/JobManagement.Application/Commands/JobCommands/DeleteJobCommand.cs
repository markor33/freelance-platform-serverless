using FluentResults;
using MediatR;
using System.Text.Json.Serialization;

namespace JobManagement.Application.Commands.JobCommands
{
    public class DeleteJobCommand : IRequest<Result>
    {
        public Guid JobId { get; private set; }

        [JsonConstructor]
        public DeleteJobCommand(Guid jobId)
        {
            JobId = jobId;
        }
    }
}
