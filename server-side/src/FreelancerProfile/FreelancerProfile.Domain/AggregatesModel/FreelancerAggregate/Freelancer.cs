using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Enums;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.SeedWork;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate
{
    public class Freelancer : EventSourcedAggregate
    {
        public Guid UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set;}
        public Contact Contact { get; private set; }
        public DateTime Joined { get; private set; }
        public int Credits { get; private set; }
        public bool IsProfilePublic { get; private set; }
        public ProfileSummary? ProfileSummary { get; private set; }
        public HourlyRate? HourlyRate { get; private set; }
        public List<LanguageKnowledge> LanguageKnowledges { get; private set; } = new();
        public Availability Availability { get; private set; }
        public ExperienceLevel ExperienceLevel { get; private set; }
        public Guid? ProfessionId { get; private set; } = null;
        public string? ProfilePictureUrl { get; private set; }
        public Profession Profession { get; private set; }
        public List<Skill> Skills { get; private set; } = new();
        public List<Education> Educations { get; private set; } = new();
        public List<Certification> Certifications { get; private set; } = new();
        public List<Employment> Employments { get; private set; } = new();
        public List<PortfolioProject> PortfolioProjects { get; private set; } = new();

        public Freelancer() { }

        public static Freelancer Create(Guid userId)
        {
            var freelancer = new Freelancer()
            {
                Id = Guid.NewGuid(),
                UserId = userId
            };

            var @event = new FreelancerCreated(freelancer.Id, userId);
            freelancer.Changes.Add(@event);

            return freelancer;
        }

        public override void Apply(DomainEvent @event)
        {
            When((dynamic)@event);
            Version = Version++;
        }

        private void Causes(DomainEvent @event)
        {
            Changes.Add(@event);
            Apply(@event);
        }

        public void SetupProfile(
            bool isProfilePublic,
            ProfileSummary profileSummary,
            HourlyRate hourlyRate,
            Availability availability,
            ExperienceLevel experienceLevel,
            Profession profession,
            LanguageKnowledge languageKnowledge)
        {
            var @event = new ProfileSetupCompleted(Id, isProfilePublic, profileSummary, hourlyRate, 
                availability, experienceLevel, profession, languageKnowledge);
            Causes(@event);

            var @creditsAdded = new CreditsAdded(Id, 100);
            Causes(@creditsAdded);
        }

        public void UpdateProfileSummary(ProfileSummary profileSummary)
        {
            var @event = new ProfileSummaryUpdated(Id, profileSummary);
            Causes(@event);
        }

        public void AddSkills(List<Skill> skills)
        {
            if (!skills.Any())
                return;

            Skills.AddRange(skills);
            var @event = new SkillsUpdated(Id, Skills);
            Changes.Add(@event);
        }

        public void RemoveSkills(List<Skill> skills)
        {
            if (!skills.Any())
                return;

            Skills.RemoveAll(s => skills.Any(rs => rs.Id == s.Id));
            var @event = new SkillsUpdated(Id, Skills);
            Changes.Add(@event);
        }

        public void AddEducation(Education education)
        {
            var @event = new EducationAdded(Id, education);
            Causes(@event);
        }

        public void UpdateEducation(Guid educationId, string schoolName, string degree, DateRange attended)
        {
            var @event = new EducationUpdated(Id, educationId, schoolName, degree, attended);
            Causes(@event);
        }

        public void DeleteEducation(Guid educationId)
        {
            var @event = new EducationDeleted(Id, educationId);
            Causes(@event);
        }

        public void AddCertification(Certification certification)
        {
            var @event = new CertificationAdded(Id, certification);
            Causes(@event);
        }

        public void UpdateCertification(Guid certificationId, string name, string provider, DateRange attended, string? description)
        {
            var @event = new CertificationUpdated(Id, certificationId, name, provider, attended, description);
            Causes(@event);
        }

        public void DeleteCertification(Guid certificationId)
        {
            var @event = new CertificationDeleted(Id, certificationId);
            Causes(@event);
        }

        public void AddEmployment(Employment employment)
        {
            var @event = new EmploymentAdded(Id, employment);
            Causes(@event);
        }

        public void UpdateEmployment(Guid employmentId, string company, string title, DateRange period, string description)
        {
            var @event = new EmploymentUpdated(Id, employmentId, company, title, period, description);
            Causes(@event);
        }

        public void DeleteEmployment(Guid employmentId)
        {
            var @event = new EmploymentDeleted(Id, employmentId); 
            Causes(@event);
        }

        public void SetProfilePicture(string profilePictureUrl)
        {
            var @event = new ProfilePictureChanged(Id, profilePictureUrl);
            Causes(@event);
        }

        public bool SubtractCredits(int credits)
        {
            if (Credits < credits) return false;

            var @event = new CreditsSubtracted(Id, credits);
            Causes(@event);

            return true;
        }

        private void When(FreelancerCreated @event)
        {
            Id = @event.AggregateId;
            UserId = @event.UserId;
        }

        private void When(ProfileSetupCompleted @event)
        {
            IsProfilePublic = @event.IsProfilePublic;
            ProfileSummary = @event.ProfileSummary;
            HourlyRate = @event.HourlyRate;
            Availability = @event.Availability;
            ExperienceLevel = @event.ExperienceLevel;
            Profession = @event.Profession;
            ProfessionId = @event.ProfessionId;
            LanguageKnowledges.Add(@event.LanguageKnowledge);
        }

        private void When(CreditsAdded @event)
        {
            Credits += @event.Amount;
        }

        private void When(CreditsSubtracted @event)
        {
            Credits -= @event.Amount;
        }

        private void When(ProfileSummaryUpdated @event)
        {
            ProfileSummary = @event.ProfileSummary;
        }

        private void When(SkillsUpdated @event)
        {
            Skills = @event.Skills;
            /*var skillsToRemove = Skills.Where(s => !@event.Skills.Any(ns => ns.Id == s.Id)).ToList();
            foreach (var skillToRemove in skillsToRemove)
                Skills.Remove(skillToRemove);

            var skillsToAdd = @event.Skills.Where(ns => !Skills.Any(s => s.Id == ns.Id)).ToList();
            Skills.AddRange(skillsToAdd);*/
        }

        private void When(EducationAdded @event)
        {
            Educations.Add(@event.Education);
        }

        private void When(EducationUpdated @event)
        {
            var education = Educations.FindById(@event.EducationId);
            education.Update(@event.SchoolName, @event.Degree, @event.Attended);
        }

        private void When(EducationDeleted @event)
        {
            var education = Educations.FindById(@event.EducationId);
            Educations.Remove(education);
        }

        private void When(CertificationAdded @event)
        {
            Certifications.Add(@event.Certification);
        }

        private void When(CertificationUpdated @event)
        {
            var certification = Certifications.FindById(@event.CertificationId);
            certification.Update(@event.Name, @event.Provider, @event.Attended, @event.Description);
        }

        private void When(CertificationDeleted @event)
        {
            var certification = Certifications.FindById(@event.CertificationId);
            Certifications.Remove(certification);
        }

        private void When(EmploymentAdded @event)
        {
            Employments.Add(@event.Employment);
        }

        private void When(EmploymentUpdated @event)
        {
            var employment = Employments.FindById(@event.EmploymentId);
            employment.Update(@event.Company, @event.Title, @event.Period, @event.Description);
        }

        private void When(EmploymentDeleted @event)
        {
            var employment = Employments.FindById(@event.EmploymentId);
            Employments.Remove(employment);
        }

        private void When(ProfilePictureChanged @event)
        {
            ProfilePictureUrl = @event.ProfilePictureUrl;
        }

    }
}
