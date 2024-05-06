using Microsoft.IdentityModel.Tokens;
using reservas.api.Models;
using reservas.api.Services.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace reservas.api.Services
{
    public class TokenService : ITokenService
    {
        public string Generate(UserModel user)
        {
            var handler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(Configuration.PrivateKey);
           
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256
                );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(2)
            };

            var token = handler.CreateToken(tokenDescriptor );
            
            return handler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateClaims(UserModel user)
        {
            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim("Id", user.Id.ToString()));
            ci.AddClaim(new Claim(ClaimTypes.Name, user.Matricula));
            ci.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
            ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            ci.AddClaim(new Claim("DataNascimento", user.DataNascimento.ToString()));
            ci.AddClaim(new Claim(ClaimTypes.Role, user.Perfil.ToString()));

            return ci;
        }
    }
}
