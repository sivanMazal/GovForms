using Microsoft.AspNetCore.Mvc;
using GovForms.Engine.Services;

namespace GovForms.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormsController : ControllerBase
    {
        private readonly WorkflowService _workflowService;

        public FormsController(WorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        [HttpPost("run-process")]
        public IActionResult RunProcess()
        {
            // הרצת המנוע החכם שלך
            _workflowService.Run();
            
            return Ok("Process started! Check the VS Code Terminal for logs.");
        }
    }
}