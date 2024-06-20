using FreelancerProfile.Domain.SeedWork;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects
{
    public class Contact : ValueObject
    {
        public Address Address { get; private set; }
        public string PhoneNumber { get; private set; }

        public Contact() { }

        [JsonConstructor]
        public Contact(Address address, string phoneNumber)
        {
            Address = address;
            PhoneNumber = phoneNumber;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Address;
            yield return PhoneNumber;
        }
    }
}
