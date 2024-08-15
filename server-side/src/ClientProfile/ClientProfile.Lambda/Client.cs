using Amazon.DynamoDBv2.DataModel;
namespace ClientProfile.Lambda;

[DynamoDBTable("ClientProfile")]
public class Client
{
    [DynamoDBHashKey]
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Contact Contact { get; set; }

    public void SetupProfile(string firstName, string lastName, Contact contact)
    {
        FirstName = firstName;
        LastName = lastName;
        Contact = contact;
    }
}

public class Contact
{
    public Address Address { get; set; }
    public string PhoneNumber { get; set; }

    public Contact()
    {
    }

    public Contact(Address address, string phoneNumber)
    {
        Address = address;
        PhoneNumber = phoneNumber;
    }
}

public class Address
{
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }
    public string ZipCode { get; set; }

    public Address()
    {
    }

    public Address(string country, string city, string street, string number, string zipCode)
    {
        Country = country;
        City = city;
        Street = street;
        Number = number;
        ZipCode = zipCode;
    }
}
