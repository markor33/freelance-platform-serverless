using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Common.Layer.Security;
using Amazon.EventBridge;
using System.Text.Json;
using Common.Layer.JsonOptions;
using Common.Layer.Headers;
using ClientProfile.Lambda.Persistence;
using FreelancerProfile.Domain.Repositories;
using WriteModel;
using ClientProfile.Lambda;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ChooseRole.Lambda;

public class Function
{
    private ILambdaContext _context;
    private readonly IAmazonCognitoIdentityProvider _cognitoClient;
    private readonly IAmazonEventBridge _eventBridgeClient = new AmazonEventBridgeClient();
    private readonly string _userPoolId = Environment.GetEnvironmentVariable("USER_POOL_ID");
    private readonly string _clientId = Environment.GetEnvironmentVariable("COGNITO_CLIENT_ID");

    private readonly IClientRepository _clientRepository;
    private readonly IFreelancerRepository _freelancerRepository;

    public Function()
    {
        _cognitoClient = new AmazonCognitoIdentityProviderClient();
        _eventBridgeClient = new AmazonEventBridgeClient();
        _clientRepository = new ClientRepository();
        _freelancerRepository = new FreelancerRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        _context = context;
        try
        {
            var command = JsonSerializer.Deserialize<RegisterUserCommand>(request.Body, JsonOptions.Options);

            await CommandHandler(command);

            return new APIGatewayProxyResponse()
            {
                StatusCode = 200,
                Headers = Headers.CORS
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"ERROR - {ex}\nSTACK TRACE - {ex.StackTrace}");
            return new APIGatewayProxyResponse()
            {
                StatusCode = 500,
                Headers = Headers.CORS,
                Body = ex.Message
            };
        }
    }

    private async Task CommandHandler(RegisterUserCommand command)
    {
        string sub = await SignUp(command.Username, command.Email, command.Password);
        await AddUserToGroup(command.Username, command.Role);

        if (command.Role == Role.Freelancer)
        {
            await CreateFreelancer(sub, command);
        }
        else
        {
            await CreateClient(sub, command);
        }
    }

    private async Task<string> SignUp(string username, string email, string password)
    {
        var signUpRequest = new SignUpRequest
        {
            ClientId = _clientId,
            Username = username,
            Password = password,
            UserAttributes = new List<AttributeType>
                {
                    new AttributeType
                    {
                        Name = "email",
                        Value = email,
                    }
                }
        };

        var response = await _cognitoClient.SignUpAsync(signUpRequest);

        if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            throw new Exception($"ERORR ON CREATING COGNITO USER");

        return response.UserSub;
    }

    private async Task<AdminAddUserToGroupResponse> AddUserToGroup(string username, Role role)
    {
        var response = await _cognitoClient.AdminAddUserToGroupAsync(new AdminAddUserToGroupRequest
        {
            UserPoolId = _userPoolId,
            Username = username,
            GroupName = role.ToString()
        });

        return response;
    }

    private async Task CreateClient(string sub, RegisterUserCommand command)
    {
        var contact = new ClientProfile.Lambda.Contact()
        {
            Address = new ClientProfile.Lambda.Address()
            {
                Country = command.Contact.Address.Country,
                City = command.Contact.Address.City,
                Street = command.Contact.Address.Street,
                Number = command.Contact.Address.Number,
                ZipCode = command.Contact.Address.ZipCode,
            },
            PhoneNumber = command.Contact.PhoneNumber
        };

        var client = new Client()
        {
            Id = Guid.Parse(sub),
            FirstName = command.FirstName,
            LastName = command.LastName,
            Contact = contact
        };

        await _clientRepository.SaveAsync(client);
    }

    private async Task CreateFreelancer(string sub, RegisterUserCommand command)
    {
        var freelancer = Freelancer.Create(Guid.Parse(sub), command.FirstName, command.LastName, command.Contact);

        await _freelancerRepository.SaveAsync(freelancer);
    }

}

public class RegisterUserCommand
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects.Contact Contact { get; set; }
    public Role Role { get; set; }
}
