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
public async Task<IActionResult> Approve(int id)
{
    // שליפת ה-RoleID של המשתמש מה-DB (נניח משתמש 1007 הוא Reviewer)
    var user = await _context.Users.FindAsync(1007); 
    
    // קריאה לפונקציה המעודכנת עם שני הפרמטרים
    await _workflowService.ApproveAsync(id, user.RoleID);
    
    return Ok(new { message = "הפעולה בוצעה" });
}

[HttpPost("submit")]
public async Task<IActionResult> SubmitApplication([FromBody] Application application)
{
    if (application == null) return BadRequest("נתוני הבקשה חסרים.");

    // 1. בדיקת מסמכים (לוגיקה ראשונית)
    if (application.AttachedDocuments == null || !application.AttachedDocuments.Any())
    {
        application.StatusID = (int)ApplicationStatus.MissingDocuments;
    }
    else
    {
        application.StatusID = (int)ApplicationStatus.WaitingForTreatment;
    }

    application.SubmissionDate = DateTime.Now;

    // 2. שמירה ראשונית בבסיס הנתונים (יצירת ה-ID לטופס)
    _context.Applications.Add(application);
    await _context.SaveChangesAsync();

    // 3. הרצת ה-Workflow - הוא זה שיעדכן את הסטטוס הסופי וירשום להיסטוריה [cite: 2026-01-13]
    await _workflowService.ProcessApplication(application);

    // *** שלב 4 נמחק! אין צורך ברישום ידני כאן כי ה-Repository כבר עשה זאת ***

    return Ok(new { 
        ApplicationId = application.Id, 
        FinalStatus = application.StatusID,
        Message = "הטופס התקבל ועובד במערכת." 
    });
}

   
    }
}