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

            //tokendaki değere göre minute veriyor. API de kullanıyor burayı
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
            //{
            //    x.ExpireTimeSpan = TimeSpan.FromSeconds(10);
            //    x.AccessDeniedPath = new PathString("/Login/PageDenied/");
            //    x.LoginPath = "/Home/Index/";
            //});
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
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
