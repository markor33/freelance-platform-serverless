using Amazon.DynamoDBv2.DataModel;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;

namespace ReadModel;

[DynamoDBTable("LanguageRepo")]
public class LanguageViewModel
{
    [DynamoDBHashKey]
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }

    public Language ToLanguage()
    {
        return new Language(Id, Name, ShortName);
    }

}

