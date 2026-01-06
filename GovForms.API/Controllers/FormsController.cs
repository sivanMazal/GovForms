using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovForms.Engine.Services;
using GovForms.API.DTOs;
using GovForms.Engine.Data;
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;

namespace GovForms.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormsController : ControllerBase
    {
        private readonly WorkflowService _workflowService;
        private readonly IAppRepository _repository;

        public FormsController(WorkflowService workflowService, IAppRepository repository)
        {
            _workflowService = workflowService;
            _repository = repository;
        }

        [HttpPost("run-process")]
        public async Task<IActionResult> RunProcess()
        {
await _workflowService.RunAsync();            return Ok("Process started! Check logs.");
        }

        [HttpPost]
        public IActionResult CreateApplication([FromBody] ApplicationRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // טריק להמרת String ל-Enum
            // אם המשתמש שלח משהו לא הגיוני, נשים ברירת מחדל (Building)
            ApplicationType typeEnum;
            if (!Enum.TryParse(request.Type, out typeEnum))
            {
                typeEnum = ApplicationType.BuildingPermit; // ברירת מחדל
            }

            var newApp = new Application
            {
                Title = request.Title,
                Amount = request.Amount,
                
                // התיקון: שימוש במשתנה המומר
                Type = typeEnum,
                
                UserId = 1,
                SubmissionDate = DateTime.Now,
                Status = ApplicationStatus.NotSubmitted,
                AttachedDocuments = new List<ApplicationDocument>()
            };

            var createdApp = _repository.AddApplication(newApp);

            return CreatedAtAction(nameof(GetStatus), new { id = createdApp.Id }, createdApp);
        }

        [HttpGet("{id}")]
        public IActionResult GetStatus(int id)
        {
            var app = _repository.GetApplicationById(id);
            if (app == null) return NotFound();
            return Ok(app);
        }
    }
}