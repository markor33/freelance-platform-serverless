using EducationCommands.Lambda.Commands;
using FluentValidation;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.Repositories;
using Moq;
using Shouldly;

namespace FreelancerProfile.IntegrationTests;

[Collection("Education")]
public class EducationScenario : BaseIntegrationTest
{
    private readonly Mock<IFreelancerRepository> _freelancerRepository = new();

    private static readonly Guid EducationId = Guid.NewGuid();

    public EducationScenario(DependecyFixture fixture) : base(fixture)
    {
        var freelancer = DependecyFixture.GetTestFreelnacer();
        CreateTestEducation(freelancer);
        _freelancerRepository.Setup(x => x.GetByIdAsync(DependecyFixture.FreelancerId)).ReturnsAsync(freelancer);
    }

    [Fact]
    public async Task Add_Education_ReturnsOk()
    {
        var validatorMock = new Mock<IValidator<AddEducationCommand>>();
        var handler = new AddEducationCommandHandler(_freelancerRepository.Object, validatorMock.Object, fixture.LambdaContext);

        var result = await handler.CommandHandler(GetTestAddEducationCommand());

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Update_Education_ReturnsOk()
    {
        var validatorMock = new Mock<IValidator<UpdateEducationCommand>>();
        var handler = new UpdateEducationCommandHandler(_freelancerRepository.Object, validatorMock.Object, fixture.LambdaContext);

        var result = await handler.CommandHandler(GetTestUpdateEducationCommand());

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Delete_Education_ReturnsOk()
    {
        var validatorMock = new Mock<IValidator<DeleteEducationCommand>>();
        var handler = new DeleteEducationCommandHandler(_freelancerRepository.Object, validatorMock.Object, fixture.LambdaContext);

        var result = await handler.CommandHandler(GetTestDeleteEducationCommand());

        result.IsSuccess.ShouldBeTrue();
    }

    private static AddEducationCommand GetTestAddEducationCommand()
        => new()
        {
            FreelancerId = DependecyFixture.FreelancerId,
            SchoolName = "FTN",
            Degree = "Dipl .ing",
            Start = DateTime.Now,
            End = DateTime.Now.AddMonths(5),
        };

    private static UpdateEducationCommand GetTestUpdateEducationCommand()
        => new()
        {
            FreelancerId = DependecyFixture.FreelancerId,
            EducationId = EducationId,
            SchoolName = "schoolName",
            Degree = "degree",
            Start = DateTime.Now,
            End = DateTime.Now.AddMonths(5),
        };

    private static DeleteEducationCommand GetTestDeleteEducationCommand()
        => new(DependecyFixture.FreelancerId, EducationId);

    private static void CreateTestEducation(Freelancer freelancer)
    {
        var education = new Education(EducationId, "schoolName", "degree", new DateRange(DateTime.Now, DateTime.Now.AddMonths(1)));
        freelancer.AddEducation(education);
    }
}
