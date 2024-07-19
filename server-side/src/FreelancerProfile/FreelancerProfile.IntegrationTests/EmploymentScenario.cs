using EmploymentCommands.Lambda.Commands;
using FluentValidation;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.Repositories;
using Moq;
using Shouldly;

namespace FreelancerProfile.IntegrationTests;

[Collection("Employment")]
public class EmploymentScenario : BaseIntegrationTest
{
    private readonly Mock<IFreelancerRepository> _freelancerRepository = new();

    private static readonly Guid EmploymentId = Guid.NewGuid();

    public EmploymentScenario(DependecyFixture fixture) : base(fixture)
    {
        var freelancer = DependecyFixture.GetTestFreelnacer();
        CreateTestEmployment(freelancer);
        _freelancerRepository.Setup(x => x.GetByIdAsync(DependecyFixture.FreelancerId)).ReturnsAsync(freelancer);
    }

    [Fact]
    public async Task Add_Employment_ReturnsOk()
    {
        var validatorMock = new Mock<IValidator<AddEmploymentCommand>>();
        var handler = new AddEmploymentCommandHandler(fixture.FreelancerRepository, validatorMock.Object, fixture.LambdaContext);

        var result = await handler.CommandHandler(GetTestAddEmploymentCommand());

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Update_Employment_ReturnsOk()
    {
        var validatorMock = new Mock<IValidator<UpdateEmploymentCommand>>();
        var handler = new UpdateEmploymentCommandHandler(_freelancerRepository.Object, validatorMock.Object, fixture.LambdaContext);

        var result = await handler.CommandHandler(GetTestUpdateEmploymentCommand());

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Delete_Employment_ReturnsOk()
    {
        var validatorMock = new Mock<IValidator<DeleteEmploymentCommand>>();
        var handler = new DeleteEmploymentCommandHandler(_freelancerRepository.Object, validatorMock.Object, fixture.LambdaContext);

        var result = await handler.CommandHandler(GetTestDeleteEmploymentCommand());

        result.IsSuccess.ShouldBeTrue();
    }

    private static AddEmploymentCommand GetTestAddEmploymentCommand()
            => new()
            {
                FreelancerId = DependecyFixture.FreelancerId,
                Company = "Microsoft",
                Title = "Software Engineer",
                Start = DateTime.Now,
                End = DateTime.Now.AddMonths(5),
                Description = "Desc"
            };

    private static UpdateEmploymentCommand GetTestUpdateEmploymentCommand()
        => new()
        {
            FreelancerId = DependecyFixture.FreelancerId,
            EmploymentId = EmploymentId,
            Company = "ABC Corp",
            Title = "Software Engineer",
            Start = new DateTime(2020, 1, 1),
            End = new DateTime(2023, 12, 31),
            Description = "Developed and maintained various software applications."
        };

    private static DeleteEmploymentCommand GetTestDeleteEmploymentCommand()
        => new(DependecyFixture.FreelancerId, EmploymentId);

    private static void CreateTestEmployment(Freelancer freelancer)
    {
        var employment = new Employment(EmploymentId, "company", "title", new DateRange(DateTime.Now, DateTime.Now.AddMonths(1)), "desc");
        freelancer.AddEmployment(employment);
    }

}
