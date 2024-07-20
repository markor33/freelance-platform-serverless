using Amazon.Lambda.Core;
using ClientProfile.Lambda.Handlers;
using ClientProfile.Lambda.IntegrationEvents;
using ClientProfile.Lambda.Persistence;
using Common.Layer.EventBus;
using Moq;
using Shouldly;

namespace ClientProfile.IntegrationTests;

public class CreateClientCommandHandlerTests : IDisposable
{
    private readonly IClientRepository _repository;
    private Mock<ILambdaContext> _context = new();
    private Mock<ILambdaLogger> _logger = new();

    private static Guid ClientId = Guid.NewGuid();

    public CreateClientCommandHandlerTests()
    {
        _repository = new ClientRepository();
        _context.Setup(x => x.Logger).Returns(_logger.Object);
    }

    [Fact]
    public async Task CreateClient_Ok()
    {
        var @event = new EventBusEvent<ClientRegisteredIntegrationEvent>()
        {
            Detail = new ClientRegisteredIntegrationEvent()
            {
                UserId = ClientId
            }
        };
        var handler = new CreateClientCommandHandler(_repository, _context.Object);

        await handler.FunctionHandler(@event, _context.Object);

        var client = await _repository.GetByIdAsync(ClientId);
        client.ShouldNotBeNull();
    }

    public void Dispose()
    {
        Task.Run(() => _repository.DeleteAsync(ClientId)).Wait();
    }
}
