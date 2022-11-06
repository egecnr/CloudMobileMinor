using Microsoft.IdentityModel.Tokens;

namespace ShowerShow.Authorization
{
    public class TokenValidationParametersJWT : TokenValidationParameters
    {
        public TokenValidationParametersJWT(string Issuer, string Audience, SymmetricSecurityKey SecurityKey)
        {
            RequireSignedTokens = true;
            ValidAudience = Audience;
            ValidateAudience = true;
            ValidIssuer = Issuer;
            ValidateIssuer = true;
            ValidateIssuerSigningKey = true;
            ValidateLifetime = true;
            IssuerSigningKey = SecurityKey;
            AuthenticationType = "Bearer";
        }
    }
}