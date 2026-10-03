using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

public class OktaAuthorizationAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;

        var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();

        if (authHeader == null || !authHeader.StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();

        if (!ValidateToken(token))
        {
            context.Result = new UnauthorizedResult();
        }
    }

    private bool ValidateToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "https://your-okta-domain/oauth2/default",

                ValidateAudience = true,
                ValidAudience = "api://default",

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = GetSigningKeys()
            };

            handler.ValidateToken(token, validationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private IEnumerable<SecurityKey> GetSigningKeys()
    {
        // In real app, fetch from Okta JWKS endpoint
        // https://your-okta-domain/oauth2/default/v1/keys
        return new List<SecurityKey>();
    }
}