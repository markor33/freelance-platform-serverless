using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events
{
    public class EmploymentUpdated : DomainEvent
    {
        public Guid EmploymentId { get; private set; }
        public string Company { get; private set; }
        public string Title { get; private set; }
        public DateRange Period { get; private set; }
        public string Description { get; private set; }

        [JsonConstructor]
        public EmploymentUpdated(Guid aggregateId, Guid employmentId, string company, string title, DateRange period, string description) 
            : base(aggregateId)
        {
            EmploymentId = employmentId;
            Company = company;
            Title = title;
            Period = period;
            Description = description;
        }
    }
}
