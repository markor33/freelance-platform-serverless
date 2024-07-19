using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Common.Layer.Security;

public static class TokenExtensions
{
    public static string GetUsername(this JwtSecurityToken token)
    {
        var usernameClaim = token.Claims.FirstOrDefault(c => c.Type == "username")
                            ?? throw new SecurityTokenException("Username claim not found in token");

        return usernameClaim.Value;
    }
}

