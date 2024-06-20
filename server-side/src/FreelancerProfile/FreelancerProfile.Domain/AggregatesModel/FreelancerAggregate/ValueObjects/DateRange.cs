using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects
{
    public class DateRange : ValueObject
    {
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public DateRange() { }

        [JsonConstructor]
        public DateRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Start;
            yield return End;
        }
    }
}
