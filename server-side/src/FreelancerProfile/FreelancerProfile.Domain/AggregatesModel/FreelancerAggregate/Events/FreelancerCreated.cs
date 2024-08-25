using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events
{

    public class FreelancerCreated : DomainEvent
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Contact Contact { get; set; }
        public DateTime Joined { get; set; }

        public FreelancerCreated() : base() { }

        [JsonConstructor]
        public FreelancerCreated(Guid aggregateId, Guid userId, string firstName, string lastName, Contact contact, DateTime joined) : base(aggregateId)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Contact = contact;
            Joined = joined;
        }

    }
}
