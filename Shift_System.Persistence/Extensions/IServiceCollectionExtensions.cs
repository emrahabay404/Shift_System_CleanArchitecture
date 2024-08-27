using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Persistence.Contexts;
using Shift_System.Persistence.Repositories;

namespace Shift_System.Persistence.Extensions
{
    public static class IServiceCollectionExtensions
    {
        //IConfiguration configuration = builder.Configuration;

        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration);
            services.AddRepositories();
        }

        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            //var connectionString = configuration.GetConnectionString("Ms_Sql_Conn");
            //var connectionString = "Server=MYPC\\SQLEXPRESS;Database=Shift_Db33;Trusted_Connection=true;TrustServerCertificate=True;";
            var connectionString = configuration["ConnectionStrings:Ms_Sql_Conn"];
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString,
      builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
                            .AddEntityFrameworkStores<ApplicationDbContext>()
                            .AddDefaultTokenProviders();

            //services.AddJwtAuthentication();
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