using Microsoft.AspNetCore.Identity;
using Shift_System.Domain.Entities.Tables;

namespace Shift_System.Domain.Entities.Models
{
    public class RoleAssignmentViewModel
    {
        public List<AppUser> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public List<UserRolesViewModel> UserRoles { get; set; }  // Yeni eklenen özellik
    }
}