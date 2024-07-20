using Amazon.Lambda.Core;
using ClientProfile.Lambda.IntegrationEvents;
using ClientProfile.Lambda.Persistence;
using Common.Layer.EventBus;

namespace ClientProfile.Lambda.Handlers;

public class CreateClientCommandHandler
{
    private readonly IClientRepository _repository;
    private ILambdaContext _context;

    public CreateClientCommandHandler()
    {
        _repository = new ClientRepository();
    }

    public CreateClientCommandHandler(IClientRepository repository, ILambdaContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task FunctionHandler(EventBusEvent<ClientRegisteredIntegrationEvent> @event, ILambdaContext context)
    {
        _context = context;
        try
        {
            var client = new Client()
            {
                Id = @event.Detail.UserId
            };
            await _repository.SaveAsync(client);

            context.Logger.LogInformation($"Freelancer created: {client}");
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Exception: {ex}");
        }
    }
}
