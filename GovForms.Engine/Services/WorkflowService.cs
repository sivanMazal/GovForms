using System;
using System.Collections.Generic;
using GovForms.Engine.Data;
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;
using GovForms.Engine.Validators; // <--- הוספנו את הגישה לתיקייה החדשה

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
            Console.WriteLine("Starting STRATEGY workflow process...");
            
            // הגדרות משתמש (סימולציה)
            UserRole currentUserRole = UserRole.Clerk; 
            int currentUserId = 555;

            var apps = _repository.GetApplicationsByStatus((int)ApplicationStatus.NotSubmitted);

            foreach (var app in apps)
            {
                Console.WriteLine($" -> Checking App #{app.Id} ({app.Type})...");

                // --- שלב 1: בדיקת אבטחה (נשאר ללא שינוי) ---
                if (app.Amount > 10000 && currentUserRole != UserRole.Manager && currentUserRole != UserRole.Admin)
                {
                    Console.WriteLine($"    [!] ACCESS DENIED (Security Block).");
                    _repository.LogHistory(new ApplicationHistory
                    {
                        ApplicationId = app.Id,
                        Status = app.Status,
                        Action = "SecurityBlock",
                        Remarks = "Amount too high for user role",
                        UserId = currentUserId
                    });
                    continue; 
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
                        Status = ApplicationStatus.InProcess,
                        Action = "StatusUpdate",
                        Remarks = "Validation passed by expert logic",
                        UserId = currentUserId
                    });
                }
                else
                {
                    Console.WriteLine("    [X] Validation Failed.");
                    _repository.UpdateStatus(app.Id, (int)ApplicationStatus.ReturnedForDocs);

                    _repository.LogHistory(new ApplicationHistory
                    {
                        ApplicationId = app.Id,
                        Status = ApplicationStatus.ReturnedForDocs,
                        Action = "ValidationFail",
                        Remarks = "Expert validator rejected the documents",
                        UserId = currentUserId
                    });
                }
            }
        }
    }
}