using System.Security.Claims;
using Freshly.API.Dtos;
using Freshly.BusinessLayer.Interfaces;
using Freshly.DomainLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace Freshly.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceTokenController : ControllerBase
    {
        private readonly IDeviceTokenService _deviceTokenService;

        public DeviceTokenController(IDeviceTokenService deviceTokenService)
        {
            _deviceTokenService = deviceTokenService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterToken([FromBody] DeviceTokenRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            await _deviceTokenService.RegisterDeviceTokenAsync(userId, request.Token, request.Platform);
            
            return Ok(new MessageResponse("Token kaydedildi."));
        }

        [HttpDelete("{token}")]
        public async Task<IActionResult> RemoveToken(string token)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            await _deviceTokenService.RemoveTokenAsync(userId, token);
            
            return Ok(new MessageResponse("Token silindi."));
        }
    }
}
