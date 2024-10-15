using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shift_System.Domain.Entities.Tables;

namespace Shift_System.Infrastructure.Services
{
    public static class RoleAndUserInitialization
    {
        public static async Task InitializeRolesAndSuperAdminAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            // SuperAdmin bilgilerini appsettings.json'dan al
            var superAdminSettings = configuration.GetSection("SuperAdminConfig");

            string roleName = superAdminSettings["Role"]; // appsettings.json'dan Role bilgisini al
            IdentityResult roleResult;

            // Rolün var olup olmadığını kontrol et, yoksa oluştur
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // SuperAdmin kullanıcı bilgilerini oluştur
            var superAdminUser = new AppUser
            {
                UserName = superAdminSettings["Username"],
                Email = superAdminSettings["Email"],
                PhoneNumber = superAdminSettings["Phone"],
                FullName = superAdminSettings["Name"] + superAdminSettings["Surname"]
            };

            // SuperAdmin kullanıcıyı kontrol et ve oluştur
            var user = await userManager.FindByEmailAsync(superAdminSettings["Email"]);
            if (user == null)
            {
                var createSuperAdmin = await userManager.CreateAsync(superAdminUser, superAdminSettings["Password"]);
                if (createSuperAdmin.Succeeded)
                {
                    // Kullanıcıya rol ata
                    await userManager.AddToRoleAsync(superAdminUser, roleName);
                }
            }
            else
            {
                // Eğer kullanıcı zaten varsa, rolünü kontrol et ve eksikse ekle
                var rolesForUser = await userManager.GetRolesAsync(user);
                if (!rolesForUser.Contains(roleName))
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
    }
}