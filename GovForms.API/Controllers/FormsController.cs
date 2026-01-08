using Microsoft.AspNetCore.Mvc;
using GovForms.Engine.Services;
using GovForms.API.DTOs;
using GovForms.Engine.Data;
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;
using GovForms.Engine.Interfaces;

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

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitApplication([FromBody] Application application)
        {
            if (application == null) return BadRequest("נתוני הבקשה חסרים.");

            // בדיקת מסמכים אוטומטית (דרישה מס' 2) [cite: 2025-12-30]
            if (application.AttachedDocuments == null || !application.AttachedDocuments.Any())
            {
                application.Status = ApplicationStatus.MissingDocuments;
            }
            else
            {
                application.Status = ApplicationStatus.WaitingForTreatment;
            }

            application.SubmissionDate = DateTime.Now;

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            // תיעוד היסטורי - התאמה למודל המקצועי [cite: 2026-01-08]
            var historyEntry = new ApplicationHistory
            {
                ApplicationId = application.Id,
                UserId = application.UserId,
                Status = application.Status,
                Action = "Submission", // במקום ActionDescription
                Timestamp = DateTime.Now, // במקום ActionDate
                Remarks = application.Status == ApplicationStatus.MissingDocuments 
                          ? "המערכת זיהתה חוסר במסמכים" 
                          : "הבקשה הוגשה בהצלחה"
            };

            _context.ApplicationHistory.Add(historyEntry);
            await _context.SaveChangesAsync();

            return Ok(new { ApplicationId = application.Id, Status = application.Status.ToString() });
        }

        [HttpPatch("{id}/approve")]
        public async Task<IActionResult> ApproveApplication(int id, [FromQuery] int currentUserId)
        {
            // בדיקת הרשאות (RBAC) [cite: 2025-12-30]
            bool canApprove = await _permissionService.CanUserApprove(currentUserId);
            if (!canApprove) return Forbid("אין לך הרשאות מתאימות.");

            var application = await _context.Applications.FindAsync(id);
            if (application == null) return NotFound("הבקשה לא נמצאה.");

            application.Status = ApplicationStatus.Treated;

            var history = new ApplicationHistory
            {
                ApplicationId = id,
                UserId = currentUserId,
                Status = ApplicationStatus.Treated,
                Action = "Approval",
                Timestamp = DateTime.Now,
                Remarks = "הבקשה אושרה לאחר בדיקה תקינה"
            };

            _context.ApplicationHistory.Add(history);
            await _context.SaveChangesAsync();

            return Ok("הבקשה אושרה בהצלחה.");
        }
    }
}