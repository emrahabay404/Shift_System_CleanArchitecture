using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shift_System.Application.Interfaces.Repositories;
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

      //private static void AddMappings(this IServiceCollection services)
      //{
      //    services.AddAutoMapper(Assembly.GetExecutingAssembly());
      //}

      public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
      {
         var connectionString = configuration.GetConnectionString("Shift_Db_Conn");

         services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

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