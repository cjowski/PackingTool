using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CoreService = PackingTool.Core.Service.User;

namespace PackingTool.Service.Service.User
{
    public class TokenService : CoreService.ITokenService
    {
        private string _apiKey { get; }

        public TokenService(
            IConfiguration config
        )
        {
            _apiKey = config.GetRequiredSection("Token:ApiKey").Value!;
        }

        public string GenerateToken(int userID)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_apiKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim("id", userID.ToString())
                    }
                ),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int ValidateToken(string token)
        {
            if (token == null)
            {
                return 0;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_apiKey);

            try
            {
                tokenHandler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, 
                    out SecurityToken validatedToken
                );

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(
                    jwtToken.Claims.First(x => x.Type == "id").Value
                );

                return userId;
            }
            catch
            {
                return 0;
            }
        }
    }
}
