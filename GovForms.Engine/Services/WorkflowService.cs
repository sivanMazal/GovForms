using System;
using System.Threading.Tasks;
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;
using GovForms.Engine.Data;
using Microsoft.EntityFrameworkCore;
using GovForms.Engine.Interfaces; // עבור INotificationService [cite: 2025-12-30]
      // עבור GovFormsDbContext [cite: 2026-01-08]
namespace GovForms.Engine.Services
{
    public class WorkflowService
    {
        private readonly IAppRepository _repository;
        private readonly INotificationService _notification; // הוספה [cite: 2025-12-30]

        public WorkflowService(IAppRepository repository, INotificationService notification)
        {
            _repository = repository;
            _notification = notification; // הזרקה [cite: 2025-12-30]
        }
public async Task ApproveAsync(int applicationId)
{
    // הוספת await כאן פותרת את שגיאות CS1061 [cite: 2026-01-08]
    var app = await _repository.GetApplicationById(applicationId); 

    if (app != null)
    {
        app.StatusID = 4; // עכשיו זה יעבוד כי 'app' הוא אובייקט אמיתי ולא Task
        
        // הוספת await כאן פותרת את אזהרת CS4014 [cite: 2026-01-11]
        await _repository.UpdateStatus(applicationId, 4);

        await _notification.SendStatusUpdate(applicationId, app.UserId, "הטופס אושר!");

        await _repository.LogHistory(new ApplicationHistory { /* ... */ });
    }
}
public async Task ProcessApplication(Application app)
{
    Console.WriteLine($"--> Processing Application #{app.Id} for {app.UserEmail}");

    // שלב 1: החלטה על ניתוב לפי סכום (דרישה מס' 7) [cite: 2025-12-30]
    string historyRemarks;
    if (app.Amount > 10000)
    {
        app.StatusID = 6; // 'ממתין לבדיקה ידנית' [cite: 2026-01-11]
        historyRemarks = $"סכום חריג ({app.Amount}). הועבר לאישור מנהל בכיר (1008).";
    }
    else
    {
        app.StatusID = 2; // 'ממתין לטיפול' [cite: 2026-01-11]
        historyRemarks = $"הבקשה התקבלה ותקינה. הועברה לבודק ממשלתי.";
    }

    // שלב 2: עדכון ה-SQL (חובה להוסיף await!) [cite: 2026-01-08]
    await _repository.UpdateStatus(app.Id, app.StatusID);

    // שלב 3: תיעוד מלא ב-Audit System (דרישה מס' 4) [cite: 2025-12-30]
    await _repository.LogHistory(new ApplicationHistory
    {
        ApplicationId = app.Id,
        Action = "System Validation",
        Status = (ApplicationStatus)app.StatusID, 
        Remarks = historyRemarks,
        UserId = app.UserId,
        Timestamp = DateTime.Now
    });

    // שלב 4: שליחת התראה אוטומטית (דרישה מס' 5) [cite: 2025-12-30]
    await _notification.SendStatusUpdate(app.Id, app.UserId, $"עדכון: {historyRemarks}");
}
        // הפונקציה הישנה שלך (למקרה שהשתמשת בה)
        public async Task RunAsync()
        {
            await Task.CompletedTask;
        }
    }
}