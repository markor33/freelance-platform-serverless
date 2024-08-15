using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace Common.Layer.Extensions;

public static class JwtSecurityTokenExtensions
{
    public static string GetRole(this JwtSecurityToken jwtToken)
    {
        var groupsClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "cognito:groups");

        return groupsClaim.Value;
    }
}
