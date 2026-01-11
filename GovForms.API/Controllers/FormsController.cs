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

[HttpPatch("{id}/approve")]
public async Task<IActionResult> Approve(int id) // עכשיו רק ה-ID הוא שדה חובה! [cite: 2026-01-11]
{
    // במערכת אמיתית, אנחנו מושכים את ה-ID מה-Claims (הזהות המחוברת) [cite: 2025-12-30]
    // כרגע, לצורך הבדיקה, נגדיר אותו כמשתנה פנימי או נמשוך מה-Header
    int currentUserId = 1007; // המזהה של Israel Israeli מה-SQL שלך [cite: 2026-01-11]

    // בדיקת הרשאות (Reviewer = 1005) [cite: 2026-01-11]
    var hasPermission = await _permissionService.CheckRole(currentUserId, 1005); 
    
    if (!hasPermission)
    {
        return StatusCode(403, new { message = "גישה נדחתה: חסרות הרשאות בודק ממשלתי." });
    }

    await _workflowService.ApproveAsync(id);
    return Ok(new { message = "הטופס אושר בהצלחה", ApplicationId = id });
}

[HttpPost("submit")]
public async Task<IActionResult> SubmitApplication([FromBody] Application application)
{
    if (application == null) return BadRequest("נתוני הבקשה חסרים.");

    // בדיקת מסמכים אוטומטית (דרישה מס' 2) [cite: 2025-12-30]
    if (application.AttachedDocuments == null || !application.AttachedDocuments.Any())
    {
        application.StatusID = (int)ApplicationStatus.MissingDocuments; // שימוש ב-StatusID [cite: 2026-01-08]
    }
    else
    {
        application.StatusID = (int)ApplicationStatus.WaitingForTreatment;
    }

    application.SubmissionDate = DateTime.Now;

    _context.Applications.Add(application);
    await _context.SaveChangesAsync();

    // תיעוד היסטורי (דרישה מס' 4 - Audit System) [cite: 2025-12-30]
    var historyEntry = new ApplicationHistory
    {
        ApplicationId = application.Id,
        UserId = application.UserId,
Status = (ApplicationStatus)application.StatusID,        Action = "Submission",
        Timestamp = DateTime.Now,
        Remarks = application.StatusID == (int)ApplicationStatus.MissingDocuments 
                  ? "המערכת זיהתה חוסר במסמכים" 
                  : "הבקשה הוגשה בהצלחה"
    };

    _context.ApplicationHistory.Add(historyEntry);
    await _context.SaveChangesAsync();

    return Ok(new { ApplicationId = application.Id, Status = application.StatusID });
}
    }
}