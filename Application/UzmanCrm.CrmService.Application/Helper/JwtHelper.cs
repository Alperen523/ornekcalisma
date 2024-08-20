using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoginService.Model;
using UzmanCrm.CrmService.Domain.Entity.CRM.Login;

namespace UzmanCrm.CrmService.Application.Helper
{
    public static class JwtHelper
    {

        public static TokenResponse CreateToken(string username, List<RoleDto> roles)
        {
            try
            {

                var tokenModel = new TokenResponse();
                var issuedAt = DateTime.Now;
                var expires = DateTime.Now.AddMinutes(1440);
                var tokenHandler = new JwtSecurityTokenHandler();

                var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) });

                claimsIdentity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role.RoleName)));

                var now = DateTime.UtcNow;
                var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(StaticConsts.JwtSecurityKey));
                var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

                var token =
                    tokenHandler.CreateJwtSecurityToken(issuer: StaticConsts.JwtIssuer,
                        audience: StaticConsts.JwtAudience,
                        subject: claimsIdentity, notBefore: issuedAt, expires: expires,
                        signingCredentials: signingCredentials
                        );
                var tokenString = tokenHandler.WriteToken(token);

                tokenModel.Token = tokenString;
                tokenModel.Expires_in = expires;

                return tokenModel;
            }
            catch (Exception e)
            {

                return null;


            }

        }

    }
}
