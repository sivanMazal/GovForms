using Microsoft.AspNetCore.Mvc;
using GovForms.Engine.Services;
using GovForms.API.DTOs;
using GovForms.Engine.Data;
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;
using GovForms.Engine.Interfaces;
using Microsoft.EntityFrameworkCore; // <--- השורה הזו חסרה לך!

namespace GovForms.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormsController : ControllerBase
    {
        private readonly WorkflowService _workflowService;
        private readonly IAppRepository _repository;
        private readonly IPermissionService _permissionService;
        private readonly GovFormsDbContext _context; // הוספנו את השדה החסר

        // בנאי אחד מאוחד - הפתרון לשגיאה CS1520 [cite: 2026-01-08]
        public FormsController(
            WorkflowService workflowService,
            IAppRepository repository,
            IPermissionService permissionService,
            GovFormsDbContext context)
        {
            _workflowService = workflowService;
            _repository = repository;
            _permissionService = permissionService;
            _context = context;
        }

        [HttpPatch("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            // שליפת ה-RoleID של המשתמש מה-DB (נניח משתמש 1007 הוא Reviewer)
            var user = await _context.Users.FindAsync(1007);

            // קריאה לפונקציה המעודכנת עם שני הפרמטרים
            await _workflowService.ApproveAsync(id, user.RoleID);

            return Ok(new { message = "הפעולה בוצעה" });
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitApplication([FromBody] ApplicationRequestDto request)
        {
            // המרה מ-DTO למודל SQL (Entity) [cite: 2026-01-13]
            var application = new Application
            {
                Title = request.Title,
                Amount = request.Amount,
                UserEmail = request.Email, // המיפוי שומר על הסדר
                UserId = request.UserId,
                SubmissionDate = DateTime.Now,
                StatusID = (int)ApplicationStatus.WaitingForTreatment
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            await _workflowService.ProcessApplication(application);

            return Ok(new { Id = application.Id, Status = "Submitted" });
        }

        [HttpGet("queue")]
        public async Task<IActionResult> GetWorkQueue([FromQuery] int? statusId, [FromQuery] decimal? minAmount)
        {
            var query = _context.Applications.AsQueryable();

            // לוגיקה עסקית לסינון (נפוץ במערכות גדולות) [cite: 2025-12-30]
            if (statusId.HasValue)
                query = query.Where(a => a.StatusID == statusId.Value);

            if (minAmount.HasValue)
                query = query.Where(a => a.Amount >= minAmount.Value);

            var results = await query
                .OrderByDescending(a => a.SubmissionDate)
                .ToListAsync();

            return Ok(results);
        }
    }
}