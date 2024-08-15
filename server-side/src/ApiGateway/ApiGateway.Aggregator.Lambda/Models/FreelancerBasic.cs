using ReadModel;

namespace ApiGateway.Aggregator.Lambda.Models;

public class FreelancerBasic
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Country { get; private set; }
    public string City { get; private set; }

    public FreelancerBasic(FreelancerViewModel freelancer)
    {
        Id = freelancer.Id;
        FirstName = freelancer.FirstName;
        LastName = freelancer.LastName;
        Country = freelancer.Contact.Address.Country;
        City = freelancer.Contact.Address.City;
    }
}
