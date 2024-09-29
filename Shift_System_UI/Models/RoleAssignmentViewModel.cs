using Microsoft.AspNetCore.Identity;
using Shift_System.Domain.Entities;

namespace Shift_System_UI.Models
{
    public class RoleAssignmentViewModel
    {
        public List<AppUser> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public List<UserRolesViewModel> UserRoles { get; set; }  // Yeni eklenen özellik
    }
}