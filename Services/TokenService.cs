using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using e_commerce_course_api.Entities;
using e_commerce_course_api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace e_commerce_course_api.Services
{
    /// <summary>
    /// Service for generating JWT tokens.
    /// </summary>
    /// <param name="userManager">
    /// The user manager.
    /// </param>
    /// <param name="config">
    /// The configuration.
    /// </param>
    public class TokenService(UserManager<User> userManager, IConfiguration config) : ITokenService
    {
        /// <summary>
        /// The user manager.
        /// </summary>
        private readonly UserManager<User> _userManager = userManager;

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IConfiguration _config = config;

        /// <inheritdoc/>
        public async Task<string> GenerateTokenAsync(User user)
        {
            var email = user.Email;
            var userName = user.UserName;
            var tokenKey = _config["JWTSettings:TokenKey"];

            if (email is null || userName is null || tokenKey is null)
            {
                throw new Exception("Error generating token.");
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, email),
                new(ClaimTypes.Name, userName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
