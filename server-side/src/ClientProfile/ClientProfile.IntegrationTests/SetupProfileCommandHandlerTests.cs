using Amazon.Lambda.Core;
using ClientProfile.Lambda;
using ClientProfile.Lambda.Handlers;
using ClientProfile.Lambda.Persistence;
using FluentValidation;
using Moq;
using Shouldly;

namespace ClientProfile.IntegrationTests;

public class SetupProfileCommandHandlerTests : IDisposable
{
    private readonly IClientRepository _repository;
    private readonly Mock<IValidator<SetupProfileCommand>> _validator = new();
    private readonly Mock<ILambdaContext> _context = new();
    private readonly Mock<ILambdaLogger> _logger = new();

    private static Guid ClientId = Guid.NewGuid();

    public SetupProfileCommandHandlerTests()
    {
        _repository = new ClientRepository();
        _context.Setup(x => x.Logger).Returns(_logger.Object);
    }

    [Fact]
    public async Task SetupProfile_Ok()
    {
        var client = new Client()
        {
            Id = ClientId
        };
        await _repository.SaveAsync(client);
        var handler = new SetupProfileCommandHandler(_repository, _validator.Object, _context.Object);

        var result = await handler.CommandHandler(GetTestSetupProfileCommand());

        result.ShouldBeTrue();
    }

    private static SetupProfileCommand GetTestSetupProfileCommand()
        => new()
        {
            ClientId = ClientId,
            FirstName = "John",
            LastName = "Johnes",
            Contact = new Contact(new Address("country", "city", "street", "number", "zipcode"), "01231231232"),
        };

    public async void Dispose()
    {
        Task.Run(() => _repository.DeleteAsync(ClientId)).Wait();
    }
}
