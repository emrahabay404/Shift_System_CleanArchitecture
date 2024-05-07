using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Shift_System.Application.Extensions
{
   public static class JwtExtensions
   {

      public const string SecurityKey = "ByYM000OLlMQG6VVVp1OH7Xzyr7gHuw1qvUC5dcGt3SNM";

      public static void AddJwtAuthentication(this IServiceCollection services)
      {
         services.AddAuthentication(opt =>
         {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         })
         .AddJwtBearer("Bearer", options =>
         {
            options.TokenValidationParameters = new TokenValidationParameters
            {
               ValidateIssuer = true,
               ValidIssuer = "https://localhost:7157",
               ValidateAudience = false,
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey))
            };
         });

       
         services.AddMvc(config =>
         {
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            config.Filters.Add(new AuthorizeFilter(policy));
         });



      }


      //public static void AddJwtAuthentication(this IServiceCollection services)
      //{
      //   services.AddAuthentication(options =>
      //   {
      //      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      //      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      //      options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      //   })
      //   .AddJwtBearer(options =>
      //   {
      //      options.SaveToken = true;
      //      options.RequireHttpsMetadata = false;
      //      options.TokenValidationParameters = new TokenValidationParameters
      //      {
      //         ValidateIssuer = true,
      //         ValidateAudience = true,
      //         ValidAudience = "https://localhost:7157",
      //         ValidIssuer = "https://localhost:7157",
      //         ValidateIssuerSigningKey = true,
      //         ClockSkew = TimeSpan.Zero,
      //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey))
      //      };
      //   });
      //}



   }
}