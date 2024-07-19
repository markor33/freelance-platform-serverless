using Amazon.Lambda.Core;
using Amazon.S3;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate;
using FreelancerProfile.Domain.Repositories;
using Moq;

namespace FreelancerProfile.IntegrationTests;

public class DependecyFixture
{
    private readonly Mock<IFreelancerRepository> _freelancerRepository = new();
    private readonly Mock<IProfessionRepository> _professionRepository = new();
    private readonly Mock<ILanguageRepository> _languageRepository = new();
    private readonly Mock<IAmazonS3> _amazonS3 = new();
    private Mock<ILambdaContext> _context = new();
    private Mock<ILambdaLogger> _logger = new();

    public static readonly Guid FreelancerId = Guid.NewGuid();
    public static readonly Guid ProfessionId = Guid.NewGuid();
    public static readonly int LanguageId = 1;

    public IFreelancerRepository FreelancerRepository => _freelancerRepository.Object;
    public IProfessionRepository ProfessionRepository => _professionRepository.Object;
    public ILanguageRepository LanguageRepository => _languageRepository.Object;
    public ILambdaContext LambdaContext => _context.Object;

    public DependecyFixture()
    {
        _freelancerRepository.Setup(x => x.GetByIdAsync(FreelancerId)).ReturnsAsync(GetTestFreelnacer());
        _professionRepository.Setup(x => x.GetByIdAsync(ProfessionId)).ReturnsAsync(GetTestProfession());
        _languageRepository.Setup(x => x.GetByIdAsync(LanguageId)).ReturnsAsync(GetTestLanguage());
        _context.Setup(x => x.Logger).Returns(_logger.Object);
    }

    public static Freelancer GetTestFreelnacer()
        => Freelancer.Create(FreelancerId);

    public static Profession GetTestProfession()
        => new Profession(ProfessionId, "name", "desc", new List<Skill>());

    public static Language GetTestLanguage()
        => new Language(LanguageId, "name", "shortName");
}
