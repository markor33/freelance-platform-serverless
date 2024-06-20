using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;

namespace ReadModel;

public class SkillViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public Skill ToSkill()
        => new(Id, Name, Description);

    public static List<Skill> ToSkills(List<SkillViewModel> skills)
        => new(skills.Select(s => s.ToSkill()));
}
