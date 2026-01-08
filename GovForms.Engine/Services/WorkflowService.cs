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

        public async Task ProcessApplication(Application app)
        {
            Console.WriteLine($"--> Processing Application #{app.Id} for {app.UserEmail}");

            // חוק 1: בדיקת סכום חריג (מעל 10,000 ש"ח) [cite: 2025-12-30]
            if (app.Amount > 10000)
            {
                app.StatusId = 5; // PendingManualReview
                
                // תיעוד היסטורי (Audit Log)
                _repository.LogHistory(new ApplicationHistory
                {
                    ApplicationId = app.Id,
                    Action = "System Validation",
                    Status = ApplicationStatus.PendingManualReview,
                    Remarks = $"Amount {app.Amount} exceeds 10,000. Required manual approval.",
                    UserId = app.UserId,
                    Timestamp = DateTime.Now
                });
            }
            else
            {
                app.StatusId = 1; // InReview (תקין, ממשיך בבדיקה רגילה)
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