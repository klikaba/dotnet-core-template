using Klika.ResourceApi.Model.Interfaces.Template;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Klika.ResourceApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TemplateController : ControllerBase
    {
        public ITemplateService _templateService;
        private readonly ILogger<TemplateController> _logger;

        public TemplateController(ITemplateService templateService, ILogger<TemplateController> logger)
        {
            _templateService = templateService;
            _logger = logger;
        }

        [HttpPost("create-template")]
        public async Task<ActionResult> CreateTemplate()
        {
            try
            {
                await _templateService.Create()
                     .ConfigureAwait(false);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/create-template");
                return StatusCode(500);
            }
        }
    }
}
