using System;
using System.Threading.Tasks;
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;
using GovForms.Engine.Data;
using Microsoft.EntityFrameworkCore;
namespace GovForms.Engine.Services
{
    public class WorkflowService
    {
        private readonly IAppRepository _repository;

        public WorkflowService(IAppRepository repository)
        {
            _repository = repository;
        }
public async Task ApproveAsync(int applicationId)
{
    // 1. שליפת הבקשה מה-SQL בבית [cite: 2026-01-08]
    var app = _repository.GetApplicationById(applicationId);
    if (app != null)
    {
        // 2. עדכון לסטטוס 'טופל' (מספר 4 לפי ה-SQL שצילמת) [cite: 2026-01-11]
        app.StatusID = 4; 
        _repository.UpdateStatus(applicationId, 4);

        // 3. תיעוד במערכת ה-Audit (דרישה מס' 4) [cite: 2025-12-30]
        _repository.LogHistory(new ApplicationHistory
        {
            ApplicationId = applicationId,
            Action = "Approval",
            Status = (ApplicationStatus)4, // המרה מפורשת ל-Enum [cite: 2026-01-08]
            Timestamp = DateTime.Now,
            Remarks = "הטופס אושר על ידי בודק ממשלתי לאחר בדיקה ידנית"
        });
    }
}
      public async Task ProcessApplication(Application app)
{
    Console.WriteLine($"--> Processing Application #{app.Id} for {app.UserEmail}");

    // חוק מקצועי: סכום מעל 10,000 עובר לבדיקה ידנית (סטטוס 6 בתמונה שלך) [cite: 2025-12-30]
    if (app.Amount > 10000)
    {
        app.StatusID = 6; // 'ממתין לבדיקה ידנית' לפי ה-SQL שלך [cite: 2025-12-30]
        
     _repository.LogHistory(new ApplicationHistory
{
    ApplicationId = app.Id,
    Action = "System Validation",
    // התיקון: הוספת (ApplicationStatus) לפני המשתנה [cite: 2026-01-08]
    Status = (ApplicationStatus)app.StatusID, 
    Remarks = $"סכום חריג ({app.Amount}). נדרש אישור ידני.",
    UserId = app.UserId,
    Timestamp = DateTime.Now
});
    }
    else
    {
        app.StatusID = 2; // 'ממתין לטיפול' - המשך תהליך רגיל [cite: 2025-12-30]
    }

    await Task.CompletedTask;
}
        // הפונקציה הישנה שלך (למקרה שהשתמשת בה)
        public async Task RunAsync()
        {
            await Task.CompletedTask;
        }
    }
}