using FreelancerProfile.Domain.SeedWork;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects
{
    public class Contact : ValueObject
    {
        public string TimeZoneId { get; private set; }
        public Address Address { get; private set; }
        public string PhoneNumber { get; private set; }
        private TimeZoneInfo _timeZone = null;
        public TimeZoneInfo TimeZone
        {
            get
            {
                if (_timeZone is not null)
                    return _timeZone;
                _timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
                return _timeZone;
            }
            private set { _timeZone = value; }
        }

        public Contact() { }

        [JsonConstructor]
        public Contact(string timeZoneId, Address address, string phoneNumber)
        {
            TimeZoneId = timeZoneId;
            TimeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
            Address = address;
            PhoneNumber = phoneNumber;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TimeZoneId;
            yield return Address;
            yield return PhoneNumber;
        }
    }
}
