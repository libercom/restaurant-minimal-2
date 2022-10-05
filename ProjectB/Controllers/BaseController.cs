using Microsoft.AspNetCore.Mvc;
using ProjectB.Models;
using ProjectB.Services;

namespace ProjectB.Controllers
{
    [ApiController]
    [Route("api")]
    public class BaseController : Controller
    {
        private readonly ILogger<BaseController> _logger;
        private readonly BaseService _service;

        public BaseController(ILogger<BaseController> logger, BaseService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost("send")]
        public IActionResult Index(Client value)
        {
            _logger.LogInformation($"Recieved new client: {value.Id}");
            _service.Push(value);

            return Ok();
        }
    }
}
