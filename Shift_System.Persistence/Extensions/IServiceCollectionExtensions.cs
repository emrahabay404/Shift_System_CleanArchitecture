using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shift_System.Application.Extensions;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Persistence.Contexts;
using Shift_System.Persistence.Repositories;

namespace Shift_System.Persistence.Extensions
{
   public static class IServiceCollectionExtensions
   {
      public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
      {
         //services.AddMappings();
         services.AddDbContext(configuration);
         services.AddRepositories();
      }

      public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
      {
         var connectionString = configuration.GetConnectionString("Shift_Db_Conn");
         services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString,
   builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

         services.AddIdentity<AppUser, IdentityRole>()
                         .AddEntityFrameworkStores<ApplicationDbContext>()
                         .AddDefaultTokenProviders();

         services.AddJwtAuthentication();
      }

      private static void AddRepositories(this IServiceCollection services)
      {
         services
           .AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork))
             .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
          .AddTransient<IEmployeeRepository, EmployeeRepository>()
          .AddTransient<ITeam_EmployeeRepository, Team_EmployeeRepository>();
      }

   }
}