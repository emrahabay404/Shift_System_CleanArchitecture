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
    [Authorize(Roles = "Manager,Admin")]
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
                    return BadRequest(Messages.Invalid_Input_TR);
                var (status, response) = await _AuthService.Login(model);
                if (status == 0)
                    return BadRequest(Messages.Login_Failed_TR);

                return Ok(new { message = Messages.Token_Created_Success_TR, success = true, token = response });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Messages.Unexpected_Error_TR);
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(Messages.Invalid_Input_TR);

                var (status, message) = await _AuthService.Registeration(model, UserRoles.User);
                if (status == 0)
                    return BadRequest(Messages.User_Registration_Failed_TR);

                return CreatedAtAction(nameof(Register), new { message = Messages.User_Registered_Successfully_TR });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Messages.Unexpected_Error_TR);
            }
        }










        //[HttpPost]
        //[Route("assign-role")]
        //public async Task<IActionResult> AssignRole([FromBody] RoleAssignmentModel model)
        //{
        //    if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.RoleName))
        //    {
        //        return BadRequest(new { success = false, message = Messages.Required_Field_Missing_TR });
        //    }

        //    var user = await _UserManager.FindByNameAsync(model.Username);
        //    if (user == null)
        //    {
        //        return NotFound(new { success = false, message = Messages.User_Not_Found_TR });
        //    }

        //    var roleExist = await _RoleManager.RoleExistsAsync(model.RoleName);
        //    if (!roleExist)
        //    {
        //        return NotFound(new { success = false, message = Messages.Role_Not_Found_TR });
        //    }

        //    var result = await _UserManager.AddToRoleAsync(user, model.RoleName);
        //    if (result.Succeeded)
        //    {
        //        return Ok(new { success = true, message = Messages.Role_Assigned_Successfully_TR });
        //    }

        //    return StatusCode(500, new { success = false, message = Messages.Role_Assignment_Failed_TR });
        //}

        //[HttpGet]
        //[Route("api/get-users")]
        //public IActionResult GetAllUsers()
        //{
        //    var users = _UserManager.Users.ToList();

        //    if (users == null || users.Count == 0)
        //    {
        //        return NotFound(new { success = false, message = Messages.User_Not_Found_TR });
        //    }

        //    var userList = users.Select(async user => new
        //    {
        //        Username = user.UserName,
        //        Email = user.Email,
        //        Roles = string.Join(",", await _UserManager.GetRolesAsync(user))
        //    }).ToList();

        //    return Ok(new { success = true, users = userList });
        //}

        //[HttpGet]
        //[Route("api/get-roles")]
        //public IActionResult GetAllRoles()
        //{
        //    var roles = _RoleManager.Roles.ToList();

        //    if (roles == null || roles.Count == 0)
        //    {
        //        return NotFound(new { success = false, message = Messages.Role_Not_Found_TR });
        //    }

        //    var roleList = roles.Select(role => new
        //    {
        //        RoleName = role.Name
        //    }).ToList();

        //    return Ok(new { success = true, roles = roleList });
        //}

    }
}
