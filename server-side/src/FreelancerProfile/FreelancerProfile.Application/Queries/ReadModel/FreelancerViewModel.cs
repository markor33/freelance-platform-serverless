using Amazon.DynamoDBv2.DataModel;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Enums;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;

namespace ReadModel;

[DynamoDBTable("FPReadModelStore")]
public class FreelancerViewModel
{
    [DynamoDBHashKey]
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Contact Contact { get; set; }
    public bool IsProfilePublic { get; set; }
    public DateTime Joined { get; set; }
    public ProfileSummary ProfileSummary { get; set; }
    public List<LanguageKnowledge> LanguageKnowledges { get; set; } = [];
    public HourlyRate HourlyRate { get; set; }
    public Availability Availability { get; set; }
    public ExperienceLevel ExperienceLevel { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public ProfessionViewModel Profession { get; set; }
    public List<SkillViewModel> Skills { get; set; } = [];
    public List<EducationViewModel> Educations { get; set; } = [];
    public List<CertificationViewModel> Certifications { get; set; } = [];
    public List<EmploymentViewModel> Employments { get; set; } = [];
}

public class EducationViewModel
{
    public Guid Id { get; set; }
    public string SchoolName { get; set; }
    public string Degree { get; set; }
    public DateRange Attended { get; set; }
}

public class CertificationViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Provider { get; set; }
    public DateRange Attended { get; set; }
    public string Description { get; set; }
}

public class EmploymentViewModel
{
    public Guid Id { get; set; }
    public string Company { get; set; }
    public string Title { get; set; }
    public DateRange Period { get; set; }
    public string Description { get; set; }

}

