using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using System.IdentityModel.Tokens.Jwt;
using Common.Layer.Security;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ChooseRole.Lambda;

public record RequestBody
{
    public Role Role { get; set; }
}

public class Function
{
    private static readonly AmazonCognitoIdentityProviderClient _cognitoClient = new();

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, "/identity-service/role")]
    public async Task<APIGatewayProxyResponse> FunctionHandler(
        [FromBody] RequestBody requestBody,
        [FromHeader(Name = "Authorization")] string token,
        ILambdaContext context)
    {
        try
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var username = jwtToken.GetUsername();

            var response = await _cognitoClient.AdminAddUserToGroupAsync(new AdminAddUserToGroupRequest
            {
                UserPoolId = "eu-central-1_yP2OhxI3R",
                Username = username,
                GroupName = requestBody.Role.ToString()
            });

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
}

