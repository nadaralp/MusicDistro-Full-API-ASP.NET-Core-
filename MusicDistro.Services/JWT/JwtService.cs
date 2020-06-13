using Microsoft.IdentityModel.Tokens;
using MusicDistro.Core.Entities.Auth;
using MusicDistro.Core.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MusicDistro.Services.JWT
{
    class JwtService : IJwtService
    {

        public string GenerateToken(string secret, string issuer, double expirationInDays, string audience, User user, IList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            IEnumerable<Claim> roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
            claims.AddRange(roleClaims);

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            DateTime expires = DateTime.Now.AddDays(Convert.ToDouble(expirationInDays));

            var token = new JwtSecurityToken(
             issuer: issuer,
             audience: audience,
             claims: claims,
             expires: expires,
             signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
