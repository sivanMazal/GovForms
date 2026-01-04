using System;
using System.Collections.Generic;
using GovForms.Engine.Data;
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;
using GovForms.Engine.Validators; // <--- הוספנו את הגישה לתיקייה החדשה
using GovForms.API.Integrations;
namespace GovForms.Engine.Services
{
    public class WorkflowService
    {
        private readonly IAppRepository _repository;
        private readonly ICitizenService _citizenService; 

        public WorkflowService(IAppRepository repository, ICitizenService citizenService)
        {
            _repository = repository;
            _citizenService = citizenService; 
        }

public async Task Run()
{
    Console.WriteLine("Starting SMART workflow process...");
    
    UserRole currentUserRole = UserRole.Clerk; 
    int currentUserId = 555;

    // שליפת הבקשות
    var apps = _repository.GetApplicationsByStatus((int)ApplicationStatus.NotSubmitted);

    foreach (var app in apps)
    {
        Console.WriteLine($" -> Checking App #{app.Id} ({app.Type})...");

        // --- בדיקת משרד הפנים (החלק שחיפשת) ---
        Console.WriteLine("    [...] Contacting Ministry of Interior...");
        
        // 1. שולחים שאילתה ומחכים לתשובה
        var citizenStatus = await _citizenService.ValidateCitizenAsync(app.UserId);

        // 2. כאן הבדיקה החדשה!
        if (!citizenStatus.CanSubmitRequest)
        {
            Console.WriteLine($"    [X] CITIZEN BLOCKED: {citizenStatus.RejectionReason}");
            
            // א. מעבירים לסטטוס "בהמתנה"
            _repository.UpdateStatus(app.Id, (int)ApplicationStatus.OnHold); 

            // ב. מתעדים עם ה-Action המיוחד
            _repository.LogHistory(new ApplicationHistory
            {
                ApplicationId = app.Id,
                Status = ApplicationStatus.OnHold,
                Action = "Suspend_MinistryDebt", // <--- הנה זה כאן
                Remarks = $"Suspended due to Ministry check: {citizenStatus.RejectionReason}",
                UserId = currentUserId
            });

            continue; // מדלגים לבקשה הבאה
        }
        
        Console.WriteLine("    [V] Citizen Approved.");

        // --- מכאן הכל ממשיך רגיל ---

        // בדיקת אבטחה
        if (app.Amount > 10000 && currentUserRole != UserRole.Manager)
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

        // בדיקת תקינות (Strategy)
        IValidator validator = ValidatorFactory.GetValidator(app.Type);
        bool isValid = true;

        if (validator != null)
        {
            isValid = validator.Validate(app);
            Console.WriteLine($"    [?] Validating using expert: {validator.GetType().Name}");
        }

        // סיום וקבלת החלטות
        if (isValid)
        {
            Console.WriteLine("    [V] Validation Passed.");
            _repository.UpdateStatus(app.Id, (int)ApplicationStatus.InProcess);
            
            _repository.LogHistory(new ApplicationHistory
            {
                ApplicationId = app.Id,
                Status = ApplicationStatus.InProcess,
                Action = "StatusUpdate",
                Remarks = "Validation passed",
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
                Remarks = "Documents missing",
                UserId = currentUserId
            });
        }
    }
}
    }}