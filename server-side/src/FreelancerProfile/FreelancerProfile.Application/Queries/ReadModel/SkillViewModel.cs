using Amazon.DynamoDBv2.DataModel;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;

namespace ReadModel;

[DynamoDBTable("SkillRepo")]
public class SkillViewModel
{
    [DynamoDBHashKey]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public Skill ToSkill()
        => new(Id, Name, Description);

    public static List<Skill> ToSkills(List<SkillViewModel> skills)
        => new(skills.Select(s => s.ToSkill()));

    public static List<SkillViewModel> FromSkills(List<Skill> skills)
        => skills.Select(s => new SkillViewModel()
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
        }).ToList();

}
