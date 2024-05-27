using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events
{
    public class FreelancerCreated : DomainEvent
    {
        public Guid UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Contact Contact { get; private set; }

        [JsonConstructor]
        public FreelancerCreated(Guid aggregateId, Guid userId, string firstName, string lastName, Contact contact) : base(aggregateId)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Contact = contact;
        }

    }
}
