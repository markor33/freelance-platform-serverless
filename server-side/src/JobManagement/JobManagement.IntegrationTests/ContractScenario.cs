using JobManagement.Application.Commands.ContractCommands;
using Shouldly;

namespace JobManagement.IntegrationTests;

public class ContractScenario : BaseIntegrationTest
{
    public ContractScenario(DependecyFixture dependecyFixture) : base(dependecyFixture) { }

    [Fact]
    public async Task Create_Contract_ReturnsOk()
    {
        var commandHandler = new MakeContractCommandHandler(fixture.JobRepository);

        var result = await commandHandler.Handle(GetTestMakeContractCommand(), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Finish_Contract_ReturnsOk()
    {
        var contractId = fixture.CreateContract();
        var command = new FinishContractCommand(DependecyFixture.JobId, contractId);
        var commandHandler = new FinishContractCommandHandler(fixture.JobRepository, fixture.EventBridge);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Terminate_Contract_ReturnsOk()
    {
        var contractId = fixture.CreateContract();
        var command = new TerminateContractCommand(DependecyFixture.JobId, contractId);
        var commandHandler = new TerminateContractCommandHandler(fixture.JobRepository);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    private static MakeContractCommand GetTestMakeContractCommand()
        => new MakeContractCommand(DependecyFixture.JobId, DependecyFixture.ProposalId);

}
