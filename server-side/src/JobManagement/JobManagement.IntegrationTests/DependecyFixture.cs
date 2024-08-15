using Amazon.EventBridge;
using JobManagement.Domain.AggregatesModel.JobAggregate;
using JobManagement.Domain.AggregatesModel.JobAggregate.Entities;
using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;
using JobManagement.Domain.Repositories;
using Moq;

namespace JobManagement.IntegrationTests;

public class DependecyFixture
{
    private readonly Mock<IProfessionRepository> _professionRepository = new();
    private readonly Mock<IJobRepository> _jobRepository = new();
    private readonly Mock<IAmazonEventBridge> _evenBridge = new();

    public static readonly Guid BlankJobId = Guid.NewGuid();
    public static readonly Guid JobId = Guid.NewGuid();
    public static readonly Guid ProposalId = Guid.NewGuid();
    public static Guid ContractId = Guid.NewGuid();
    public static readonly Guid FreelancerId = Guid.NewGuid();
    public static readonly Guid ClientId = Guid.NewGuid();
    public static readonly Guid ProfessionId = Guid.NewGuid();
    public static readonly int LanguageId = 1;

    public IJobRepository JobRepository => _jobRepository.Object;
    public IProfessionRepository ProfessionRepository => _professionRepository.Object;
    public IAmazonEventBridge EventBridge => _evenBridge.Object;

    public DependecyFixture()
    {
        _jobRepository.Setup(x => x.GetByIdAsync(JobId)).ReturnsAsync(GetTestJob());
        _jobRepository.Setup(x => x.GetByIdAsync(BlankJobId)).ReturnsAsync(GetTestBlankJob());
        _professionRepository.Setup(x => x.GetByIdAsync(ProfessionId)).ReturnsAsync(GetTestProfession());
    }

    public static Job GetTestJob()
    {
        var job = Job.Create(ClientId, "Title", "Desc", ExperienceLevel.JUNIOR, new Payment(100, "USD", PaymentType.FIXED_RATE), GetTestProfession(), [], []);
        
        var proposal = new Proposal(ProposalId, FreelancerId, "Text", new Payment(100, "USD", PaymentType.FIXED_RATE), ProposalStatus.CLIENT_APPROVED, [], DateTime.UtcNow);
        job.AddProposal(proposal);

        var proposal1 = new Proposal(Guid.NewGuid(), Guid.NewGuid(), "Text", new Payment(100, "USD", PaymentType.FIXED_RATE), ProposalStatus.CLIENT_APPROVED, [], DateTime.UtcNow);
        job.AddProposal(proposal1);
        var res = job.MakeContract(proposal1.Id);
        ContractId = res.Value.Id;

        return job;
    }

    public static Job GetTestBlankJob()
    {
        var job = Job.Create(ClientId, "Title", "Desc", ExperienceLevel.JUNIOR, new Payment(100, "USD", PaymentType.FIXED_RATE), GetTestProfession(), [], []);

        return job;
    }

    public static Profession GetTestProfession()
        => new Profession(ProfessionId, "name", "desc", new List<Skill>());

}
