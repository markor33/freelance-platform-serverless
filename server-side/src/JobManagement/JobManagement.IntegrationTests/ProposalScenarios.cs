using JobManagement.Application.Commands.ProposalCommands;
using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;
using Shouldly;

namespace JobManagement.IntegrationTests;

public class ProposalScenarios : BaseIntegrationTest
{
    public ProposalScenarios(DependecyFixture dependecyFixture) : base(dependecyFixture) { }

    [Fact]
    public async Task Create_Proposal_ReturnsOk()
    {
        var commandHandler = new CreateProposalCommandHandler(fixture.JobRepository);

        var result = await commandHandler.Handle(GetTestCreateProposalCommand(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Create_ProposalAlreadyCreated_ReturnsBadRequest()
    {
        var commandHandler = new CreateProposalCommandHandler(fixture.JobRepository);

        var result = await commandHandler.Handle(GetTestCreateProposalCommand(DependecyFixture.FreelancerId), CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task Approve_Proposal_ReturnsOk()
    {
        var commandHandler = new ApproveProposalCommandHandler(fixture.JobRepository);

        var result = await commandHandler.Handle(GetTestApproveProposalCommand(), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Update_Proposal_Payment_ReturnsOk()
    {
        var commandHandler = new UpdateProposalPaymentCommandHandler(fixture.JobRepository);

        var result = await commandHandler.Handle(GetTestUpdateProposalPaymentCommand(), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    private static CreateProposalCommand GetTestCreateProposalCommand(Guid freelancerId)
        => new(freelancerId, DependecyFixture.JobId, "Test", new Payment(300, "USD", PaymentType.FIXED_RATE), []);

    private static ApproveProposalCommand GetTestApproveProposalCommand()
        => new(DependecyFixture.JobId, DependecyFixture.ProposalId);

    private static UpdateProposalPaymentCommand GetTestUpdateProposalPaymentCommand()
        => new(DependecyFixture.JobId, DependecyFixture.ProposalId, new Payment(50, "USD", PaymentType.FIXED_RATE));

}
