using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SWD.Data.DTOs.Authentication;
using SWD.Data.Entities;
using SWD.Service.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace SWD.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<User> _roleManager;
        public AuthService(UserManager<User> userManager, IConfiguration configuration, RoleManager<User> roleManager)
        {
            _userManager = userManager; 
            _configuration = configuration;
            _roleManager = roleManager;
        }
        public IActionResult GoogleLogin()
        {
            var redirectUrl = "/api/v1/auth/google-response";
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return new ChallengeResult(GoogleDefaults.AuthenticationScheme, properties);
        }

        public async Task<IActionResult> GoogleResponse(HttpContext httpContext)
        {
            var result = await httpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded) return new BadRequestObjectResult("Google authentication failed.");

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
                return new BadRequestObjectResult("Email not found.");

            // Find or create the user
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                
                user = new User // Use your custom User class
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true, // Assuming Google email is verified
                    Status = "Active", // Optional: Set a default status
                    CreatedAt = DateTime.UtcNow, // Optional: Set creation timestamp
                    LastEdited = DateTime.UtcNow, // Optional: Set last edited timestamp
                    SubscriptionStatus = "Free" // Optional: Set a default subscription status
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    return new BadRequestObjectResult(new { Errors = createResult.Errors.Select(e => e.Description) });
                }

                // Assign the existing "USER" role (assumes it exists with Name = "USER")
                var addRoleResult = await _userManager.AddToRoleAsync(user, "USER");
                if (!addRoleResult.Succeeded)
                {
                    return new BadRequestObjectResult(new { Errors = addRoleResult.Errors.Select(e => e.Description) });
                }
            }

            // Generate JWT token
            var token = GenerateToken(user);

            return new OkObjectResult(new { Email = email, Name = name, Token = token });
        }

        public async Task<string> GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

            
        };
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}
