using DemoApp.Application.DTOs.Auth;
using DemoApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DemoApp.Application.DTOs.Auth;

namespace DemoApp.API.Controllers
{
    using FluentValidation;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
    private readonly IAuthService _authService;
    private readonly UserManager<Microsoft.AspNetCore.Identity.IdentityUser> _userManager;
    private readonly RoleManager<Microsoft.AspNetCore.Identity.IdentityRole> _roleManager;
    private readonly IValidator<LoginRequest> _loginValidator;
    private readonly IValidator<RegisterRequest> _registerValidator;

    public AuthController(
            IAuthService authService,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IValidator<LoginRequest> loginValidator,
            IValidator<RegisterRequest> registerValidator)
        {
            _authService = authService;
            _userManager = userManager;
            _roleManager = roleManager;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            // Validate request
            var regValidation = _registerValidator.Validate(request);

            if (!regValidation.IsValid)
            {
                foreach (var err in regValidation.Errors)
                {
                    ModelState.AddModelError(
                        err.PropertyName ?? string.Empty,
                        err.ErrorMessage);
                }

                return ValidationProblem(ModelState);
            }

            try
            {
                // Create User
                await _authService.RegisterAsync(request);

                return Ok(new
                {
                    Message = "User Registered Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Error = ex.Message
                });
            }
        }
      
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(
            LoginRequest request)
        {
            // Validate login request
            var loginValidation = _loginValidator.Validate(request);
            if (!loginValidation.IsValid)
            {
                foreach (var err in loginValidation.Errors)
                    ModelState.AddModelError(err.PropertyName ?? string.Empty, err.ErrorMessage);

                return ValidationProblem(ModelState);
            }

            try
            {
                var response = await _authService.LoginAsync(request);

                return Ok(response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
