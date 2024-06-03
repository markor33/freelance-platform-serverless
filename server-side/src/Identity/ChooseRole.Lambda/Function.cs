using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using System.IdentityModel.Tokens.Jwt;
using Common.Layer.Security;
using System.Net;
using Amazon.SimpleNotificationService.Model;
using Amazon.EventBridge;
using Amazon.Auth.AccessControlPolicy;
using Amazon.EventBridge.Model;
using System.Text.Json;
using EventBus;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ChooseRole.Lambda;

public record RequestBody
{
    public Role Role { get; set; }
}

public record FreelancerRegisteredIntegrationEvent(Guid UserId)
{
    public Guid UserId { get; private set; } = UserId;
}

public class Function
{
    private ILambdaContext _context;

    private readonly AmazonCognitoIdentityProviderClient _cognitoClient = new();
    private readonly IAmazonEventBridge _eventBridgeClient = new AmazonEventBridgeClient();

    private readonly string _userPoolId = Environment.GetEnvironmentVariable("USER_POOL_ID");

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        _context = context;
        try
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"]);
            var username = jwtToken.GetUsername();
            var sub = jwtToken.Subject;

            if (await IsUserAlreadyInGroup(username))
            {
                return new APIGatewayProxyResponse()
                {
                    Body = "User is already in a group",
                    StatusCode = 400
                };
            }

            var requestBody = JsonSerializer.Deserialize<RequestBody>(request.Body);

            var response = await AddUserToGroup(username, requestBody.Role);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                await PublishIntegrationEvent(requestBody.Role, sub);
            }

            return new APIGatewayProxyResponse()
            {
                StatusCode = ((int)response.HttpStatusCode),
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError(ex.Message);
            return new APIGatewayProxyResponse()
            {
                StatusCode = 500,
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } },
                Body = ex.Message
            };
        }
    }

    private async Task<bool> IsUserAlreadyInGroup(string username)
    {
        var groupsResponse = await _cognitoClient.AdminListGroupsForUserAsync(new AdminListGroupsForUserRequest
        {
            UserPoolId = _userPoolId,
            Username = username
        });

        return groupsResponse.Groups.Any();
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

    private async Task PublishIntegrationEvent(Role role, string sub)
    {
        var @event = new FreelancerRegisteredIntegrationEvent(Guid.Parse(sub));

        var response = await _eventBridgeClient.PublishEvent<FreelancerRegisteredIntegrationEvent>(@event);

        if (response.FailedEntryCount > 0)
        {
            _context.Logger.LogError($"Failed to publish integration event: {response.Entries[0].ErrorCode} - {response.Entries[0].ErrorMessage}");
        }
        else
        {
            _context.Logger.LogLine($"Event sent to EventBridge with ID: {response.Entries[0].EventId}");
        }
    }
}

