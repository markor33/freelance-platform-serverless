using CertificationCommands.Lambda.Commands;
using FluentValidation;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.Repositories;
using Moq;
using Shouldly;

namespace FreelancerProfile.IntegrationTests;

public class CertificationScenario : BaseIntegrationTest
{
    private readonly Mock<IFreelancerRepository> _freelancerRepository = new();

    private static readonly Guid CertificationId = Guid.NewGuid();

    public CertificationScenario(DependecyFixture fixture) : base(fixture)
    {
        var freelancer = DependecyFixture.GetTestFreelnacer();
        CreateTestCertification(freelancer);
        _freelancerRepository.Setup(x => x.GetByIdAsync(DependecyFixture.FreelancerId)).ReturnsAsync(freelancer);
    }

    [Fact]
    public async Task Add_Certification_ReturnsOk()
    {
        var validatorMock = new Mock<IValidator<AddCertificationCommand>>();
        var handler = new AddCertificationCommandHandler(_freelancerRepository.Object, validatorMock.Object, fixture.LambdaContext);

        var result = await handler.CommandHandler(GetTestAddCertificationCommand());

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Delete_Certification_ReturnsOk()
    {
        var validatorMock = new Mock<IValidator<UpdateCertificationCommand>>();
        var handler = new UpdateCertificationCommandHandler(_freelancerRepository.Object, validatorMock.Object, fixture.LambdaContext);

        var result = await handler.CommandHandler(GetTestUpdateCertificationCommand());

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Update_Certification_ReturnsOk()
    {
        var validatorMock = new Mock<IValidator<DeleteCertificationCommand>>();
        var handler = new DeleteCertificationCommandHandler(_freelancerRepository.Object, validatorMock.Object, fixture.LambdaContext);

        var result = await handler.CommandHandler(GetTestDeleteCertificationCommand());

        result.IsSuccess.ShouldBeTrue();
    }

    private static AddCertificationCommand GetTestAddCertificationCommand()
        => new()
        {
            FreelancerId = DependecyFixture.FreelancerId,
            Name = "Certified Kubernetes Administrator",
            Provider = "The Linux Foundation",
            Description = "Certification demonstrating knowledge of Kubernetes administration.",
            Start = DateTime.Now,
            End = DateTime.Now.AddMonths(5)
        };

    private static UpdateCertificationCommand GetTestUpdateCertificationCommand()
        => new()
        {
            FreelancerId = DependecyFixture.FreelancerId,
            CertificationId = CertificationId,
            Name = "name",
            Provider = "provider",
            Description = "desc",
            Start = DateTime.Now,
            End = DateTime.Now.AddMonths(5)
        };

    private static DeleteCertificationCommand GetTestDeleteCertificationCommand()
        => new(DependecyFixture.FreelancerId, CertificationId);

    private static void CreateTestCertification(Freelancer freelancer)
    {
        var certification = new Certification(CertificationId, "name", "provider", new DateRange(DateTime.Now, DateTime.Now.AddMonths(1)), "desc");
        freelancer.AddCertification(certification);
    }
}
