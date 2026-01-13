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
        private readonly IExternalIntegrationService _externalService;

        public WorkflowService(IAppRepository repository, INotificationService notification,IExternalIntegrationService external)
        {
            _repository = repository;
            _notification = notification; // הזרקה [cite: 2025-12-30]
            _externalService = external;
        }
public async Task ApproveAsync(int applicationId, int approverRoleId)
{
    var app = await _repository.GetApplicationById(applicationId);
    if (app == null) return;

    // דרישה 7: אישור רב-שלבי לפי סכום [cite: 2025-12-30]
    
    // תרחיש א': סכום גבוה (מעל 50,000) ואישור של בודק רגיל בלבד
    if (app.Amount >= 50000 && approverRoleId == (int)UserRole.Reviewer)
    {
        // מעבירים לסטטוס 6 - ממתין לבדיקה ידנית (בכירה)
        await _repository.UpdateStatus(applicationId, (int)ApplicationStatus.PendingManualReview);
        await _notification.SendStatusUpdate(app.Id, app.UserId, "הטופס נבדק וממתין לאישור סופי של מנהל בכיר.");
    }
    // תרחיש ב': סכום נמוך או אישור של אדמין (מנהל בכיר)
    else if (app.Amount < 50000 || approverRoleId == (int)UserRole.Admin)
    {
        // מעבירים לסטטוס 4 - טופל (מאושר סופית)
        await _repository.UpdateStatus(applicationId, (int)ApplicationStatus.Treated);
        await _notification.SendStatusUpdate(app.Id, app.UserId, "בקשתך אושרה סופית!");
    }
}
public async Task ProcessApplication(Application app)
{
    Console.WriteLine($"--> Processing Application #{app.Id}");

    // 1. בדיקת חובות חיצונית [cite: 2025-12-30]
    bool hasDebts = await _externalService.HasOutstandingDebtsAsync(app.UserEmail);
    if (hasDebts)
    {
        // קריאה אחת שמעדכנת סטטוס + היסטוריה + הערה [cite: 2026-01-13]
        await _repository.UpdateStatus(app.Id, 8, "נדחה אוטומטית: נמצאו חובות במרשם האוכלוסין.");
        await _notification.SendStatusUpdate(app.Id, app.UserId, "בקשתך נדחתה עקב חובות.");
        return;
    }

    // 2. ניתוב לפי סכום (דרישה מס' 7) [cite: 2026-01-13]
    int finalStatus = (app.Amount > 10000) ? 6 : 2;
    string remarks = (finalStatus == 6) 
        ? $"סכום חריג ({app.Amount:N0}). הועבר לאישור מנהל." 
        : "הבקשה תקינה ועברה לבדיקה רגילה.";

    // 3. עדכון סופי - הפקודה היחידה שצריך! [cite: 2026-01-13]
    // הפקודה הזו מעדכנת את הטופס וגם יוצרת שורת היסטוריה אחת נקייה
    await _repository.UpdateStatus(app.Id, finalStatus, remarks);

    // 4. שליחת התראה [cite: 2025-12-30]
    await _notification.SendStatusUpdate(app.Id, app.UserId, $"עדכון סטטוס: {remarks}");
}

// *** מחקי את כל פונקציית LogAudit מכאן! ***
        // הפונקציה הישנה שלך (למקרה שהשתמשת בה)
        public async Task RunAsync()
        {
            await Task.CompletedTask;
        }
    }
}