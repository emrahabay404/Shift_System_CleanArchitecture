using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Interfaces;
using Shift_System.Domain.Entities.Models;
using Shift_System.Domain.Entities.Tables;
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
        [AllowAnonymous]
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

    }
}
