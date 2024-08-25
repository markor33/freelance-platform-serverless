using Amazon.DynamoDBv2.DataModel;
using JobManagement.Domain.AggregatesModel.JobAggregate.Entities;
using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;

namespace ReadModel;

[DynamoDBTable("JobReadModelStore")]
public record JobViewModel
{
    public Guid Id { get; private init; }
    public Guid ClientId { get; private init; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; private init; }
    public ExperienceLevel ExperienceLevel { get; set; }
    public Payment Payment { get; set; }
    public int Credits { get; set; }
    public int NumOfProposals { get; set; }
    public int CurrentlyInterviewing { get; set; }
    public int NumOfActiveContracts { get; set; }
    public int NumOfFinishedContracts { get; set; }
    public JobStatus Status { get; set; }
    public List<QuestionViewModel> Questions { get; set; }
    public ProfessionViewModel Profession { get; set; }
    public List<SkillViewModel> Skills { get; set; }

    public JobViewModel()
    {
        Questions = new List<QuestionViewModel>();
        Skills = new List<SkillViewModel>();
    }

    public JobViewModel(Guid id, Guid clientId, string title, string description, DateTime created, ExperienceLevel experienceLevel, JobStatus status,
        Payment payment, int credits, List<QuestionViewModel> questions, ProfessionViewModel profession, List<SkillViewModel> skills,
        int numOfProposals, int currentlyInterviewing, int numOfActiveContracts, int numOfFinishedContracts)
    {
        Id = id;
        ClientId = clientId;
        Title = title;
        Description = description;
        Created = created;
        ExperienceLevel = experienceLevel;
        Payment = payment;
        Credits = credits;
        Questions = questions;
        Profession = profession;
        Skills = skills;
        Status = status;
        NumOfProposals = numOfProposals;
        CurrentlyInterviewing = currentlyInterviewing;
        NumOfActiveContracts = numOfActiveContracts;
        NumOfFinishedContracts = numOfFinishedContracts;
    }
}

public record AnswerViewModel
{
    public Guid Id { get; private init; }
    public QuestionViewModel Question { get; set; }
    public string Text { get; private init; }

    public AnswerViewModel() { }

    public AnswerViewModel(Guid id, QuestionViewModel question, string text)
    {
        Id = id;
        Question = question;
        Text = text;
    }

    public AnswerViewModel(Answer answer, QuestionViewModel question)
    {
        Id = answer.Id;
        Text = answer.Text;
        Question = question;
    }
}

public class QuestionViewModel : IEquatable<QuestionViewModel>
{
    public Guid Id { get; private init; }
    public string Text { get; private init; }

    public QuestionViewModel() { }

    public QuestionViewModel(Guid id)
    {
        Id = id;
    }

    public QuestionViewModel(Guid id, string text)
    {
        Id = id;
        Text = text;
    }

    public QuestionViewModel(Question question)
    {
        Id = question.Id;
        Text = question.Text;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((QuestionViewModel)obj);
    }

    public bool Equals(QuestionViewModel? other)
    {
        if (other == null) return false;
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

}
