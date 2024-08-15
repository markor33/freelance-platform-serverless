using JobManagement.Application.Commands.JobCommands;
using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;
using Shouldly;

namespace JobManagement.IntegrationTests;

public class JobScenario : BaseIntegrationTest
{
    public JobScenario(DependecyFixture dependecyFixture) : base(dependecyFixture) { }

    [Fact]
    public async Task Create_Job_ReturnsOk()
    {
        var commandHandler = new CreateJobCommandHandler(fixture.JobRepository, fixture.ProfessionRepository);

        var result = await commandHandler.Handle(GetTestCreateJobCommand(), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Update_Job_ReturnsOk()
    {
        var commandHandler = new UpdateJobCommandHandler(fixture.JobRepository, fixture.ProfessionRepository);

        var result = await commandHandler.Handle(GetTestUpdateJobCommand(), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Job_Done_ReturnsOk()
    {
        var commandHandler = new JobDoneCommandHandler(fixture.JobRepository);

        var result = await commandHandler.Handle(GetTestJobDoneCommand(DependecyFixture.BlankJobId), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Job_Done_HasActiveContracts_ReturnsBadRequest()
    {
        var commandHandler = new JobDoneCommandHandler(fixture.JobRepository);

        var result = await commandHandler.Handle(GetTestJobDoneCommand(DependecyFixture.JobId), CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task Job_Delete_ReturnsOk()
    {
        var commandHandler = new DeleteJobCommandHandler(fixture.JobRepository);

        var result = await commandHandler.Handle(GetTestDeleteJobCommand(DependecyFixture.BlankJobId), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Job_Delete_HasContracts_ReturnsBadRequest()
    {
        var commandHandler = new DeleteJobCommandHandler(fixture.JobRepository);

        var result = await commandHandler.Handle(GetTestDeleteJobCommand(DependecyFixture.JobId), CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
    }

    private static CreateJobCommand GetTestCreateJobCommand()
        => new(Guid.NewGuid(), "Title", "Desc", ExperienceLevel.JUNIOR, new Payment(100, "USD", PaymentType.FIXED_RATE), [], DependecyFixture.ProfessionId, []);

    private static UpdateJobCommand GetTestUpdateJobCommand()
        => new(DependecyFixture.JobId, "Title", "Desc", ExperienceLevel.JUNIOR, new Payment(100, "USD", PaymentType.FIXED_RATE), [], DependecyFixture.ProfessionId, []);

    private static JobDoneCommand GetTestJobDoneCommand(Guid jobId)
        => new(jobId);

    private static DeleteJobCommand GetTestDeleteJobCommand(Guid jobId)
        => new(jobId);
}
