using System.Text.Json.Serialization;

namespace JobManagement.Application.Notifications
{
    public record InterviewStageStartedNotification
    {
        public Guid FreelancerId { get; private init; }
        public Guid JobId { get; private init; }
        public string JobTitle { get; private init; }
        public Guid ProposalId { get; private init; }

        public InterviewStageStartedNotification()
        {
        }

        [JsonConstructor]
        public InterviewStageStartedNotification(Guid freelancerId, Guid jobId, string jobTitle, Guid proposalId)
        {
            FreelancerId = freelancerId;
            JobId = jobId;
            JobTitle= jobTitle;
            ProposalId= proposalId;
        }
    }
}
