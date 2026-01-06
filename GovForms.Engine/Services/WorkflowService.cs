using System;
using System.Collections.Generic;
using GovForms.Engine.Data;
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;
using GovForms.Engine.Validators; // <--- הוספנו את הגישה לתיקייה החדשה
using System.Threading.Tasks;
namespace GovForms.Engine.Services
{
    public class WorkflowService
    {
        private readonly IAppRepository _repository;

        public WorkflowService(IAppRepository repository)
        {
            _repository = repository;
        }

public async Task RunAsync()        {
            Console.WriteLine("Starting STRATEGY workflow process...");
            
            // הגדרות משתמש (סימולציה)
            UserRole currentUserRole = UserRole.Clerk; 
            int currentUserId = 555;

            var apps = _repository.GetApplicationsByStatus((int)ApplicationStatus.NotSubmitted);

            foreach (var app in apps)
            {
                Console.WriteLine($" -> Checking App #{app.Id} ({app.Type})...");

                // --- שלב 1: בדיקת אבטחה (נשאר ללא שינוי) ---
                // --- שלב 1: בדיקת אבטחה (RBAC) ---
// שלב 1: בדיקת סמכות (RBAC) - האם הסכום גבוה מדי לתפקיד הנוכחי?
if (app.Amount > 10000 && currentUserRole != UserRole.Manager && currentUserRole != UserRole.Admin)
{
    Console.WriteLine($"    [!] Security: App #{app.Id} requires Manager approval (Amount: {app.Amount})");
    
    // עדכון סטטוס ב-Repository (מחר בבית זה יעדכן את ה-SQL)
    _repository.UpdateStatus(app.Id, (int)ApplicationStatus.PendingManualReview);
    
    // רישום ב-Audit System - חובה לתיעוד לאומי [cite: 2025-12-30]
    _repository.LogHistory(new ApplicationHistory
    {
        ApplicationId = app.Id,
        Action = "SecurityCheck",
        Status = ApplicationStatus.PendingManualReview.ToString(),
        Remarks = $"Amount {app.Amount} exceeds Clerk limit. Moved to manual review.",
        UserId = currentUserId,
        Timestamp = DateTime.Now
    });

    continue; // עוצרים כאן ועוברים לבקשה הבאה
}
// שלב 2: בדיקת תנאי סף - האם צורפו מסמכים לבקשה?
if (app.AttachedDocuments == null || app.AttachedDocuments.Count == 0)
{
    Console.WriteLine($"    [X] Validation: App #{app.Id} has no documents.");

    _repository.UpdateStatus(app.Id, (int)ApplicationStatus.ReturnedForDocs);

    _repository.LogHistory(new ApplicationHistory
    {
        ApplicationId = app.Id,
        Action = "DocumentCheck",
        Status = ApplicationStatus.ReturnedForDocs.ToString(),
        Remarks = "No documents attached. Returning to applicant.",
        UserId = currentUserId,
        Timestamp = DateTime.Now
    });

    continue; // אין טעם להריץ ולידטורים אם אין מסמכים
}
                // --- שלב 2: בדיקת תקינות באמצעות ה-FACTORY החדש! ---
                
                // המנהל מבקש מהמפעל: "תביא לי בבקשה מומחה שמבין בסוג הבקשה הזה"
IValidator validator = ValidatorFactory.GetValidator(app.Type);                
                bool isValid = true;

                if (validator != null)
                {
                    // המומחה מבצע את הבדיקה
                    isValid = validator.Validate(app);
                    Console.WriteLine($"    [?] Validating using expert: {validator.GetType().Name}");
                }
                else
                {
                    // אם אין מומחה ספציפי (null), נניח שהבקשה תקינה (חוקים כלליים)
                    Console.WriteLine("    [?] No specific validator found. Using general rules.");
                }

                // --- שלב 3: קבלת החלטות (נשאר ללא שינוי) ---
              if (isValid)
{
    Console.WriteLine("    [V] Validation Passed.");
    _repository.UpdateStatus(app.Id, (int)ApplicationStatus.InProcess);
    
    _repository.LogHistory(new ApplicationHistory
    {
        ApplicationId = app.Id,
        // שימוש ב-ToString() כדי להתאים למודל ההיסטוריה
        Status = ApplicationStatus.InProcess.ToString(), 
        Action = "StatusUpdate",
        Remarks = "Validation passed by expert logic",
        UserId = currentUserId,
        Timestamp = DateTime.Now
    });
}
else
{
    Console.WriteLine("    [X] Validation Failed.");
    _repository.UpdateStatus(app.Id, (int)ApplicationStatus.ReturnedForDocs);

    _repository.LogHistory(new ApplicationHistory
    {
        ApplicationId = app.Id,
        Status = ApplicationStatus.ReturnedForDocs.ToString(),
        Action = "ValidationFail",
        Remarks = "Expert validator rejected the documents",
        UserId = currentUserId,
        Timestamp = DateTime.Now
    });
}
            }
        }
    }
}