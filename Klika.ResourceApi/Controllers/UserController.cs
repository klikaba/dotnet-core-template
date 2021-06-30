using Klika.ResourceApi.Model.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Klika.ResourceApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet("info")]
        public async Task<ActionResult> Info()
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var emailClaim = claimsIdentity.FindFirst("email");
                return Ok(emailClaim.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/info");
                return StatusCode(500);
            }
        }
    }
}
