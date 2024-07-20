using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using ClientProfile.Lambda.Persistence;
using FluentValidation;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace ClientProfile.Lambda.Handlers;

public class SetupProfileCommandHandler
{
    private readonly IClientRepository _clientRepository;
    private readonly IValidator<SetupProfileCommand> _validator;
    private ILambdaContext _context;

    public SetupProfileCommandHandler()
    {
        _clientRepository = new ClientRepository();
        _validator = new SetupProfileCommandValidator();
    }

    public SetupProfileCommandHandler(IClientRepository clientRepository, IValidator<SetupProfileCommand> validator, ILambdaContext context)
    {
        _clientRepository = clientRepository;
        _validator = validator;
        _context = context;
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        _context = context;
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"]);
        var sub = jwtToken.Subject;

        var id = request.PathParameters["id"];
        if (id != sub)
        {
            context.Logger.LogError("Profile setup failed - missing path param");

            return new APIGatewayProxyResponse()
            {
                StatusCode = 401
            };
        }

        var command = JsonSerializer.Deserialize<SetupProfileCommand>(request.Body);
        command.ClientId = Guid.Parse(sub);

        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            context.Logger.LogError($"Validation failed - {validationResult.Errors}");

            return new APIGatewayProxyResponse()
            {
                StatusCode = 400,
                Body = $"Validation failed - {validationResult.Errors}"
            };
        }

        var result = await CommandHandler(command);
        var statusCode = result ? 201 : 400;

        return new APIGatewayProxyResponse()
        {
            StatusCode = statusCode
        };
    }

    public async Task<bool> CommandHandler(SetupProfileCommand request)
    {
        try
        {
            var client = await _clientRepository.GetByIdAsync(request.ClientId);
            if (client is null)
            {
                _context.Logger.LogError($"Client with {request.ClientId} does not exist");

                return false;
            }

            client.SetupProfile(request.FirstName, request.LastName, request.Contact);
            await _clientRepository.SaveAsync(client);

            return true;
        }
        catch (Exception ex)
        {
            _context.Logger.LogError($"Exception - {ex}");

            return false;
        }
    }
}

public class SetupProfileCommand
{
    public Guid ClientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Contact Contact { get; set; }
}

public class SetupProfileCommandValidator : AbstractValidator<SetupProfileCommand>
{
    public SetupProfileCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();

        RuleFor(x => x.LastName).NotEmpty();

        RuleFor(x => x.Contact.PhoneNumber).NotEmpty();

        RuleFor(x => x.Contact.Address.Country).NotEmpty();
        RuleFor(x => x.Contact.Address.City).NotEmpty();
        RuleFor(x => x.Contact.Address.Street).NotEmpty();
        RuleFor(x => x.Contact.Address.Number).NotEmpty();
        RuleFor(x => x.Contact.Address.ZipCode).NotEmpty();
    }
}