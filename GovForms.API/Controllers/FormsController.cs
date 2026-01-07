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
            await _workflowService.RunAsync();
            return Ok("Process started! Check logs.");
        }

     [HttpPost]
public async Task<IActionResult> CreateApplication([FromBody] ApplicationRequestDto request)
{
    if (!ModelState.IsValid) return BadRequest(ModelState);

    // 1. המרת ה-Type ל-Enum
    if (!Enum.TryParse<ApplicationType>(request.Type, true, out var typeEnum))
    {
        return BadRequest($"The application type '{request.Type}' is invalid.");
    }

    // 2. יצירת האובייקט הראשוני
    var newApp = new Application 
    {
        Title = request.Title,
        Type = typeEnum,
        Amount = request.Amount,
        UserEmail = request.UserEmail,
        UserId = request.UserId,
        StatusId = 1 // סטטוס התחלתי: InReview
    };

    // 3. שמירה ראשונית ל-SQL - כאן אנחנו מקבלים את ה-Id האמיתי!
    var createdApp = _repository.AddApplication(newApp);

    // 4. עכשיו, כשיש לנו Id (למשל 5), אפשר להריץ את המנוע ולתעד היסטוריה
    await _workflowService.ProcessApplication(createdApp); 

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