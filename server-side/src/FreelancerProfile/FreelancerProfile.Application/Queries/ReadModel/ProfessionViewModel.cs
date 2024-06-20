using Amazon.DynamoDBv2.DataModel;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;

namespace ReadModel;

[DynamoDBTable("ProfessionRepo")]
public class ProfessionViewModel
{
    [DynamoDBHashKey]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<SkillViewModel> Skills { get; set; }

    public Profession ToProfession()
        => new(Id, Name, Description, SkillViewModel.ToSkills(Skills));
}
