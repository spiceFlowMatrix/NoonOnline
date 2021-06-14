using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Training24Admin.Model;

namespace Training24Admin
{
    public sealed class JwtTokenBuilder
    {
        private SecurityKey securityKey = null;
        private string subject = "";
        private string issuer = "";
        private string audience = "";
        private Dictionary<string, string> claims = new Dictionary<string, string>();
        private int expiryInMinutes = 5;

        private UserDTO userDTO = null;

        public JwtTokenBuilder AdduserDTO(UserDTO userDTO)
        {
            this.userDTO = userDTO;
            return this;
        }


        public JwtTokenBuilder AddSecurityKey(SecurityKey securityKey)
        {
            this.securityKey = securityKey;
            return this;
        }

        public JwtTokenBuilder AddSubject(string subject)
        {
            this.subject = subject;
            return this;
        }

        public JwtTokenBuilder AddIssuer(string issuer)
        {
            this.issuer = issuer;
            return this;
        }

        public JwtTokenBuilder AddAudience(string audience)
        {
            this.audience = audience;
            return this;
        }

        public JwtTokenBuilder AddClaim(string type, string value)
        {
            this.claims.Add(type, value);
            return this;
        }

        public JwtTokenBuilder AddClaims(Dictionary<string, string> claims)
        {
            this.claims.Union(claims);
            return this;
        }

        public JwtTokenBuilder AddExpiry(int expiryInMinutes)
        {
            this.expiryInMinutes = expiryInMinutes;
            return this;
        }

        public JwtToken Build()
        {
            EnsureArguments();

            List<Claim> claims = new List<Claim>
            {
                        new Claim("Id", userDTO.Id.ToString()),
                        new Claim("Email", userDTO.Email),
                        new Claim("is_skippable", userDTO.is_skippable.ToString()),
                        new Claim("timeout", userDTO.timeout.ToString()),
                        new Claim("reminder", userDTO.reminder.ToString()),
                        new Claim("istimeouton", userDTO.istimeouton.ToString())
            };

            claims.Add(new Claim("RoleName", "User"));

            foreach (var rolename in userDTO.RoleName)
            {
                claims.Add(new Claim("RoleName", rolename));
            }

            claims.Add(new Claim("RoleId", "123"));

            foreach (var roleid in userDTO.RoleId)
            {
                claims.Add(new Claim("RoleId", roleid.ToString()));
            }

            claims.Union(this.claims.Select(item => new Claim(item.Key, item.Value))).ToList();

            var token = new JwtSecurityToken(
                              issuer: this.issuer,
                              audience: this.audience,
                              claims: claims,
                              expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                              signingCredentials: new SigningCredentials(
                                                        this.securityKey,
                                                        SecurityAlgorithms.HmacSha256));

            return new JwtToken(token);
        }

        #region " private "

        private void EnsureArguments()
        {
            if (this.securityKey == null)
                throw new ArgumentNullException("Security Key");

            if (string.IsNullOrEmpty(this.subject))
                throw new ArgumentNullException("Subject");

            if (string.IsNullOrEmpty(this.issuer))
                throw new ArgumentNullException("Issuer");

            if (string.IsNullOrEmpty(this.audience))
                throw new ArgumentNullException("Audience");
        }

        #endregion
    }
}
