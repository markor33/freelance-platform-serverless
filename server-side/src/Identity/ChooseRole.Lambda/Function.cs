using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using System.IdentityModel.Tokens.Jwt;
using Common.Layer.Security;
using Amazon.SimpleNotificationService;
using System.Net;
using Amazon.SimpleNotificationService.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ChooseRole.Lambda;

public record RequestBody
{
    public Role Role { get; set; }
}

public class Function
{
    private ILambdaContext _context;

    private readonly AmazonCognitoIdentityProviderClient _cognitoClient = new();
    private readonly IAmazonSimpleNotificationService _snsClient = new AmazonSimpleNotificationServiceClient();

    private readonly string _freelancerRegisteredTopicArn = Environment.GetEnvironmentVariable("FREELANCER_REGISTERED_TOPIC_ANR");
    private readonly string _employeerRegisteredTopicArn = Environment.GetEnvironmentVariable("EMPLOYEER_DELETED_TOPIC_ARN");

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, "/identity-service/role")]
    public async Task<APIGatewayProxyResponse> FunctionHandler(
        [FromBody] RequestBody requestBody,
        [FromHeader(Name = "Authorization")] string token,
        ILambdaContext context)
    {
        _context = context;
        try
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var username = jwtToken.GetUsername();
            var sub = jwtToken.Subject;

            var response = await _cognitoClient.AdminAddUserToGroupAsync(new AdminAddUserToGroupRequest
            {
                UserPoolId = "eu-central-1_yP2OhxI3R",
                Username = username,
                GroupName = requestBody.Role.ToString()
            });

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                await PublishTopic(requestBody.Role, sub);
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

    private async Task PublishTopic(Role role, string sub)
    {
        var topicArn = role == Role.Freelancer ? _freelancerRegisteredTopicArn : _employeerRegisteredTopicArn;
        _context.Logger.LogInformation(topicArn);
        var request = new PublishRequest()
        {
            TopicArn = topicArn,
            Message = sub
        };

        var response = await _snsClient.PublishAsync(request);

        _context.Logger.LogLine($"Message sent to SNS topic: {response.MessageId}");
    }
}

