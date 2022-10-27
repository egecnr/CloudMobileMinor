using ExtraFunction.Authorization;
using ExtraFunction.Model;
using ExtraFunction.Repository_.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.Service
{
    public class TokenService :ITokenService
    {


        private string Issuer { get; }
        private string Audience { get; }
        private TimeSpan ValidityDuration { get; }

        private SigningCredentials Credentials { get; }
        private TokenValidationParametersJWT ValidationParameters { get; }

        public TokenService(IConfiguration con)
        {

            Issuer = con.GetValue<string>("JwtSettings:Issuer");
            Audience = con.GetValue<string>("JwtSettings:Audience");
            ValidityDuration = TimeSpan.FromDays(30);// Todo: configure
            string Key = con.GetValue<string>("JwtSettings:Key");

            SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);
            ValidationParameters = new TokenValidationParametersJWT(Issuer, Audience, SecurityKey);
        }
        public async Task<LoginResult> CreateToken(Login Login)
        {
            JwtSecurityToken Token = await CreateToken(new Claim[] {
            new Claim(ClaimTypes.Role, "User"),
            new Claim(ClaimTypes.Name, Login.Username)
        });

            return new LoginResult(Token);
        }

        private async Task<JwtSecurityToken> CreateToken(Claim[] Claims)
        {
            JwtHeader Header = new JwtHeader(Credentials);

            JwtPayload Payload = new JwtPayload(Issuer,
                           Audience,
                                                Claims,
                                                DateTime.UtcNow,
                                                DateTime.UtcNow.Add(ValidityDuration),
                                                DateTime.UtcNow);

            JwtSecurityToken SecurityToken = new JwtSecurityToken(Header, Payload);

            return await Task.FromResult(SecurityToken);
        }
        public async Task<ClaimsPrincipal> GetByValue(string Value)
        {
            if (Value == null)
            {
                throw new Exception("No Token supplied");
            }

            JwtSecurityTokenHandler Handler = new JwtSecurityTokenHandler();

            try
            {
                SecurityToken ValidatedToken;
                ClaimsPrincipal Principal = Handler.ValidateToken(Value, ValidationParameters, out ValidatedToken);

                return await Task.FromResult(Principal);
            }
            catch (Exception e)
            {
                throw;
            }
        }


    }
}
