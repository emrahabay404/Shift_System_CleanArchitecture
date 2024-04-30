using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shift_System.Application.Interfaces;
using Shift_System.Domain.Entities;
using Shift_System.Domain.Entities.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shift_System.Infrastructure.Services
{
   public class AuthService : IAuthService
   {
      private readonly UserManager<AppUser> userManager;
      private readonly RoleManager<IdentityRole> roleManager;
      private readonly IConfiguration _Configuration;

      public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
      {
         this.userManager = userManager;
         this.roleManager = roleManager;
         _Configuration = configuration;
      }

      public async Task<(int, string)> Registeration(RegistrationModel model, string role)
      {
         var userExists = await userManager.FindByNameAsync(model.Username);
         if (userExists != null)
            return (0, "User already exists");

         AppUser user = new()
         {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username,
            FullName = model.Name
         };

         var createUserResult = await userManager.CreateAsync(user, model.Password);
         if (!createUserResult.Succeeded)
            return (0, "User creation failed! Please check user details and try again.");

         if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));

         if (await roleManager.RoleExistsAsync(UserRoles.User))
            await userManager.AddToRoleAsync(user, role);

         return (1, "User created successfully!");
      }

      public async Task<(int, string)> Login(LoginModel model)
      {
         var user = await userManager.FindByNameAsync(model.Username);

         if (user == null)
            return (0, "Invalid username");

         if (!await userManager.CheckPasswordAsync(user, model.Password))
            return (0, "Invalid password");

         var userRoles = await userManager.GetRolesAsync(user);
         var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

         foreach (var userRole in userRoles)
         {
            //authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            authClaims.Add(new Claim("Role", userRole));
         }

         string token = GenerateToken(authClaims);
         return (1, token);
      }

      private string GenerateToken(IEnumerable<Claim> claims)
      {
         var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ByYM000OLlMQG6VVVp1OH7Xzyr7gHuw1qvUC5dcGt3SNM"));
         var tokenDescriptor = new SecurityTokenDescriptor
         {
            Issuer = "https://localhost:7157",
            Audience = "https://localhost:7157",
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims)
         };
         var tokenHandler = new JwtSecurityTokenHandler();
         var token = tokenHandler.CreateToken(tokenDescriptor);
         return tokenHandler.WriteToken(token);
      }

   }
}
