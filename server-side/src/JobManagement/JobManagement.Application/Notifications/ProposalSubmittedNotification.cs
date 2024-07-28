using System.Text.Json.Serialization;

namespace JobManagement.Application.Notifications
{
    record class ProposalSubmittedNotification 
    {
        public Guid ClientId { get; private init; }
        public Guid JobId { get; private init; }
        public string JobName { get; private init; }

        public ProposalSubmittedNotification()
        {
        }

        [JsonConstructor]
        public ProposalSubmittedNotification(Guid clientId, Guid jobId, string jobName)
        {
            ClientId = clientId;
            JobId = jobId;
            JobName = jobName;
        }

    }
}
