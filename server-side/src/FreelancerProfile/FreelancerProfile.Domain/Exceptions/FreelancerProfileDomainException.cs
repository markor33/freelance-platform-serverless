namespace FreelancerProfile.Domain.Exceptions
{
    public class FreelancerProfileDomainException : Exception
    {
        public FreelancerProfileDomainException() { }

        public FreelancerProfileDomainException(string message) : base(message) { }

        public FreelancerProfileDomainException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
