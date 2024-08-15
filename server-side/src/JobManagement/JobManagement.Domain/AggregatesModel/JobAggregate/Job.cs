using FluentResults;
using JobManagement.Domain.SeedWork;
using JobManagement.Domain.AggregatesModel.JobAggregate.Entities;
using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;
using JobManagement.Domain.AggregatesModel.JobAggregate.Events;

namespace JobManagement.Domain.AggregatesModel.JobAggregate
{
    public class Job : EventSourcedAggregate
    {
        public Guid ClientId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime Created { get; private set; }
        public int Credits { get; private set; }
        public ExperienceLevel ExperienceLevel { get; private set; }
        public Payment Payment { get; private set; }
        public JobStatus Status { get; private set; }
        public List<Question> Questions { get; private set; } = new();
        public List<Proposal> Proposals { get; private set; } = new();
        public Guid ProfessionId { get; private set; }
        public Profession Profession { get; private set; }
        public List<Skill> Skills { get; private set; } = new();
        public List<Contract> Contracts { get; private set; } = new();

        public Proposal GetProposal(Guid proposalId) => Proposals.FindById(proposalId);

        public Job() { }

        public static Job Create(Guid clientId, string title, string description, ExperienceLevel experienceLevel,
            Payment payment, Profession profession, List<Question> questions, List<Skill> skills)
        {
            var job = new Job()
            {
                Id = Guid.NewGuid(),
                ClientId = clientId,
                Title = title,
                Created = DateTime.UtcNow,
                Description = description,
                ExperienceLevel = experienceLevel,
                Payment = payment,
                Status = JobStatus.LISTED,
                Credits = EvaluateCredits(experienceLevel),
                Profession = profession,
                ProfessionId = profession.Id,
                Questions = questions,
                Skills = skills
            };

            var @event = new JobCreated(job.Id, clientId, title, description, job.Created, job.Credits, 
                experienceLevel, payment, job.Status, questions, profession.Id, skills);
            job.Changes.Add(@event);

            return job;
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

        public void Update(string title, string description, ExperienceLevel experienceLevel,
            Payment payment, Guid professionId, List<Question> questions, List<Skill> skills)
        {
            var @event = new JobUpdated(Id, title, description, EvaluateCredits(experienceLevel), experienceLevel, payment, questions, professionId, skills);
            Causes(@event);
        }

        public Result AddProposal(Proposal proposal)
        {
            var freelancerAlreadyApplied = Proposals.Any(p => p.FreelancerId == proposal.FreelancerId);
            if (freelancerAlreadyApplied)
                return Result.Fail("Freelancer already applied for this job");

            var @event = new ProposalCreated(Id, proposal, Credits);
            Causes(@event);

            return Result.Ok();
        }

        public void ChangeProposalPayment(Guid proposalId, Payment payment)
        {
            var @event = new ProposalPaymentChanged(Id, proposalId, payment);
            Causes(@event);
        }

        public void SetProposalStatusToSent(Guid proposalId)
        {
            var @event = new ProposalStatusChanged(Id, proposalId, ProposalStatus.SENT);
            Causes(@event);
        }

        public void SetProposalStatusToInterview(Guid proposalId)
        {
            var @event = new ProposalStatusChanged(Id, proposalId, ProposalStatus.INTERVIEW);
            Causes(@event);
        }

        public void SetProposalStatusToClientApproved(Guid proposalId)
        {
            var @event = new ProposalStatusChanged(Id, proposalId, ProposalStatus.CLIENT_APPROVED);
            Causes(@event);
        }

        public void RemoveProposal(Guid proposalId)
        {
            var @event = new ProposalRemoved(Id, proposalId);
            Causes(@event);
        }

        public Result<Contract> MakeContract(Guid proposalId)
        {
            var proposal = Proposals.FindById(proposalId);
            if (proposal.Status != ProposalStatus.CLIENT_APPROVED)
                return Result.Fail("Proposal is not approved by client");

            var newContract = new Contract(ClientId, proposal.FreelancerId, proposal.Payment);

            var contractCreatedEvent = new ContractCreated(Id, newContract);
            Causes(contractCreatedEvent);

            var proposalStatusChangedEvent = new ProposalStatusChanged(Id, proposal.Id, ProposalStatus.FREELANCER_APPROVED);
            Causes(proposalStatusChangedEvent);

            if (Status != JobStatus.IN_PROGRESS)
            {
                var jobStatusChangedEvent = new JobStatusChanged(Id, JobStatus.IN_PROGRESS);
                Causes(jobStatusChangedEvent);
            }

            return Result.Ok(newContract);
        }

        public Contract FinishContract(Guid contractId)
        {
            var @event = new ContractStatusChanged(Id, contractId, ContractStatus.FINISHED);
            Causes(@event);

            return Contracts.FindById(contractId);
        }

        public Contract TerminateContract(Guid contractId)
        {
            var @event = new ContractStatusChanged(Id, contractId, ContractStatus.TERMINATED);
            Causes(@event);

            return Contracts.FindById(contractId);
        }

        public Result Done()
        {
            if (!Contracts.All(c => c.Status != ContractStatus.ACTIVE))
                return Result.Fail("Job can't be done. Active contracts exist.");

            var @event = new JobDone(Id);
            Causes(@event);

            return Result.Ok();
        }

        public Result Delete()
        {
            if (Contracts.Any())
                return Result.Fail("Job can't be deleted. Active contracts exist.");

            var @event = new JobDeleted(Id);
            Causes(@event);

            return Result.Ok();
        }

        private void When(JobCreated @event)
        {
            Id = @event.AggregateId;
            ClientId = @event.ClientId;
            Title = @event.Title;
            Created = @event.Created;
            Description = @event.Description;
            ExperienceLevel = @event.ExperienceLevel;
            Payment = @event.Payment;
            Status = @event.Status;
            ProfessionId = @event.ProfessionId;
            Questions = @event.Questions;
            Skills = @event.Skills;
        }

        private void When(JobUpdated @event)
        {
            Title = @event.Title;
            Description = @event.Description;
            ExperienceLevel = @event.ExperienceLevel;
            Payment = @event.Payment;
            Credits = @event.Credits;
            ProfessionId = @event.ProfessionId;
            UpdateQuestions(@event.Questions);
        }

        private void When(JobStatusChanged @event)
        {
            Status = @event.Status;
        }

        private void When(ProposalCreated @event)
        {
            Proposals.Add(@event.Proposal);
        }

        private void When(ProposalPaymentChanged @event)
        {
            var proposal = Proposals.FindById(@event.ProposalId);
            proposal.ChangePayment(@event.Payment);
        }

        private void When(ProposalRemoved @event)
        {
            var proposal = Proposals.FindById(@event.ProposalId);
            Proposals.Remove(proposal);
        }

        private void When(ProposalStatusChanged @event)
        {
            var proposal = Proposals.FindById(@event.ProposalId);
            proposal.ChangeStatus(@event.Status);
        }

        private void When(ContractCreated @event)
        {
            Contracts.Add(@event.Contract);
        }

        private void When(ContractStatusChanged @event)
        {
            var contract = Contracts.FindById(@event.ContractId);
            contract.ChangeStatus(@event.Status);
        }

        private void When(JobDone @event)
        {
            Status = JobStatus.DONE;
            Proposals.Clear();
        }

        private void When(JobDeleted @event)
        {
            Status = JobStatus.REMOVED;
            Proposals.Clear();
        }

        private void UpdateQuestions(List<Question> questions)
        {
            var questionsToRemove = Questions.Where(q => !questions.Any(nq => nq.Id == q.Id)).ToList();
            foreach (var questionToRemove in questionsToRemove)
                Questions.Remove(questionToRemove);

            var existingQuestions = Questions.Where(q => questions.Any(nq => nq.Id == q.Id && nq.Text != q.Text)).ToList();
            foreach (var existingQuestion in existingQuestions)
            {
                var newQuestion = questions.First(nq => nq.Id == existingQuestion.Id);
                existingQuestion.SetText(newQuestion.Text);
            }

            var questionsToAdd = questions.Where(nq => !Questions.Any(q => q.Id == nq.Id)).ToList();
            Questions.AddRange(questionsToAdd);

        }

        private static int EvaluateCredits(ExperienceLevel experienceLevel) => ((int)experienceLevel + 1);

    }
}
