using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Interfaces;
using Shift_System.Domain.Entities;
using Shift_System.Domain.Entities.Models;
using Shift_System.Shared.Helpers;

namespace Shift_System.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        private readonly IAuthService _AuthService;
        private readonly UserManager<AppUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;

        public AuthController(IAuthService authService, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _AuthService = authService;
            _UserManager = userManager;
            _RoleManager = roleManager;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");
                var (status, response) = await _AuthService.Login(model);
                if (status == 0)
                    return BadRequest(response);
                //return Ok(response);
                return Ok(new { message = Messages.Token_Created_Success_TR, success = true, token = response });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("Register")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");
                var (status, message) = await _AuthService.Registeration(model, UserRoles.User);
                if (status == 0)
                {
                    return BadRequest(message);
                }
                return CreatedAtAction(nameof(Register), model);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("assign-role")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> AssignRole([FromBody] RoleAssignmentModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.RoleName))
            {
                return BadRequest(new { success = false, message = "Kullanıcı adı ve rol adı boş olamaz." });
            }

            var user = await _UserManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return NotFound(new { success = false, message = "Kullanıcı bulunamadı." });
            }

            var roleExist = await _RoleManager.RoleExistsAsync(model.RoleName);
            if (!roleExist)
            {
                return NotFound(new { success = false, message = "Rol bulunamadı." });
            }

            var result = await _UserManager.AddToRoleAsync(user, model.RoleName);
            if (result.Succeeded)
            {
                return Ok(new { success = true, message = "Rol başarıyla kullanıcıya atandı." });
            }

            return StatusCode(500, new { success = false, message = "Rol atama işlemi başarısız." });
        }

        [HttpGet]
        [Authorize(Roles = "Manager,Admin")]
        [Route("api/get-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _UserManager.Users.ToList();

            if (users == null || users.Count == 0)
            {
                return NotFound(new { success = false, message = "Kullanıcı bulunamadı." });
            }

            var userList = users.Select(async user => new
            {
                Username = user.UserName,
                Email = user.Email,
                Roles = string.Join(",", await _UserManager.GetRolesAsync(user)) // Kullanıcının tüm rollerini çekiyoruz
            }).ToList();

            return Ok(new { success = true, users = userList });
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpGet]
        [Route("api/get-roles")]
        public IActionResult GetAllRoles()
        {
            var roles = _RoleManager.Roles.ToList();

            if (roles == null || roles.Count == 0)
            {
                return NotFound(new { success = false, message = "Rol bulunamadı." });
            }

            var roleList = roles.Select(role => new
            {
                RoleName = role.Name
            }).ToList();

            return Ok(new { success = true, roles = roleList });
        }

    }
}