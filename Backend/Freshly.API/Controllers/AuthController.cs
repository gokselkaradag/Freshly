using Freshly.API.Dtos;
using Freshly.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Freshly.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request.Email, request.Password);
            
            if (!result.Success)
                return BadRequest(result.Error);
            
            return Ok(new AuthResponse(result.Token!,  result.Email!, result.UserId!.Value));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authService.LoginAsync(request.Email,  request.Password);
            
            if (!result.Success)
                return BadRequest(result.Error);
            
            return Ok(new AuthResponse(result.Token!, result.Email!, result.UserId!.Value));
        }
    }
}
