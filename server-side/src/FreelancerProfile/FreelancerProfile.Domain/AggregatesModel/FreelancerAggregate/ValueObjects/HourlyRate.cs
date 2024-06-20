using FreelancerProfile.Domain.Exceptions;
using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects
{
    public class HourlyRate : ValueObject
    {
        public float Amount { get; private set; }
        public string Currency { get; private set; }

        public HourlyRate() { }

        [JsonConstructor]
        public HourlyRate(float amount, string currency)
        {
            if (amount <= 0)
                throw new FreelancerProfileDomainException("Hourly rate amount can not be less than or equal to 0");
            Amount = amount;
            Currency = currency;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
