using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Persistence.Contexts;
using Shift_System.Persistence.Repositories;

namespace Shift_System.Persistence.Extensions
{
    public static class IServiceCollectionExtensions
    {

        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration);
            services.AddRepositories();
        }

        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["ConnectionStrings:Ms_Sql_Conn"];
            DapperContext.MyConnect = connectionString;
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString,
      builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddIdentity<AppUser, IdentityRole>(options =>
 {
     options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
     options.Lockout.MaxFailedAccessAttempts = 3;
     options.Lockout.AllowedForNewUsers = true;

     options.SignIn.RequireConfirmedAccount = false;
     options.SignIn.RequireConfirmedEmail = false;
     options.SignIn.RequireConfirmedPhoneNumber = false;

 })
                 .AddEntityFrameworkStores<ApplicationDbContext>()
                 .AddDefaultTokenProviders();
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
