using JobManagement.Domain.SeedWork;
using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using System.Text.Json.Serialization;

namespace JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects
{
    public class Payment : ValueObject
    {
        public float Amount { get; private set; }
        public string Currency { get; private set; }
        public PaymentType Type { get; private set; }

        public Payment() { }

        [JsonConstructor]
        public Payment(float amount, string currency, PaymentType type)
        {
            Amount = amount;
            Currency = currency;
            Type = type;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
            yield return Type;
        }
    }
}
