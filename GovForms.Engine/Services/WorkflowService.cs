using System;
using System.Linq;
using System.Collections.Generic;
using GovForms.Engine.Data;
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;

namespace GovForms.Engine.Services
{
    public class WorkflowService
    {
        private readonly IAppRepository _repository;

        public WorkflowService(IAppRepository repository)
        {
            _repository = repository;
        }

        public void Run()
        {
            Console.WriteLine("Starting AUDITED workflow process...");
            
            // הגדרת משתמש נוכחי (לצורך סימולציה)
            // תוכלי לשנות ל-Manager כדי לראות איך החסימה משתחררת
            UserRole currentUserRole = UserRole.Clerk; 
            int currentUserId = 555; // סתם מספר מזהה של הפקיד

            Console.WriteLine($"[Identity] User: {currentUserId} | Role: {currentUserRole}");

            // שליפת בקשות חדשות
            var apps = _repository.GetApplicationsByStatus((int)ApplicationStatus.NotSubmitted);

            foreach (var app in apps)
            {
                Console.WriteLine($" -> Checking App #{app.Id} ({app.Amount:C})...");

                // 1. בדיקת אבטחה (Security Check)
                if (app.Amount > 10000 && currentUserRole != UserRole.Manager && currentUserRole != UserRole.Admin)
                {
                    Console.WriteLine($"    [!] ACCESS DENIED. Amount too high.");
                    
                    // --- כתיבה ליומן: חסימת אבטחה ---
                    _repository.LogHistory(new ApplicationHistory
                    {
                        ApplicationId = app.Id,
                        Status = app.Status,
                        Action = "SecurityBlock",
                        Remarks = $"Role {currentUserRole} denied for amount {app.Amount}",
                        UserId = currentUserId
                    });
                    
                    continue; // מדלגים לבקשה הבאה
                }

                // 2. בדיקת לוגיקה עסקית (Validation)
                bool isValid = ValidateApplicationRules(app);

                if (isValid)
                {
                    Console.WriteLine("    [V] Validation Passed.");
                    _repository.UpdateStatus(app.Id, (int)ApplicationStatus.InProcess);
                    
                    // --- כתיבה ליומן: הצלחה ---
                    _repository.LogHistory(new ApplicationHistory
                    {
                        ApplicationId = app.Id,
                        Status = ApplicationStatus.InProcess,
                        Action = "StatusUpdate",
                        Remarks = "Validation passed. Moved to InProcess.",
                        UserId = currentUserId
                    });
                }
                else
                {
                    Console.WriteLine("    [X] Validation Failed.");
                    _repository.UpdateStatus(app.Id, (int)ApplicationStatus.ReturnedForDocs);

                    // --- כתיבה ליומן: כישלון ---
                    _repository.LogHistory(new ApplicationHistory
                    {
                        ApplicationId = app.Id,
                        Status = ApplicationStatus.ReturnedForDocs,
                        Action = "ValidationFail",
                        Remarks = "Missing documents for request type.",
                        UserId = currentUserId
                    });
                }
            }
        }

        // --- הלוגיקה החכמה (נשארת אותו דבר) ---
        private bool ValidateApplicationRules(Application app)
        {
            if (app.Documents == null || app.Documents.Count == 0) return false;

            switch (app.Type)
            {
                case ApplicationType.BuildingPermit:
                    return app.Documents.Any(d => d.Contains("Map")) && 
                           app.Documents.Any(d => d.Contains("Plan"));

                case ApplicationType.BusinessLicense:
                    return app.Documents.Any(d => d.Contains("Police"));

                case ApplicationType.TaxDiscount:
                    return app.Documents.Any(d => d.Contains("PaySlip"));

                default:
                    return true;
            }
        }
    }
}