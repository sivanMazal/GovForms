using System;
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
            Console.WriteLine("Starting workflow process...");
            
            // תיקון: שימוש בסטטוסים האמיתיים שלך
            // 1. נחפש בקשות בסטטוס "טרם הוגש" (1)
            int statusToSearch = (int)ApplicationStatus.NotSubmitted; 
            
            var apps = _repository.GetApplicationsByStatus(statusToSearch);
            
            Console.WriteLine($"Found {apps.Count} applications with status '{ApplicationStatus.NotSubmitted}'.");

            foreach (var app in apps)
            {
                Console.WriteLine($" -> Processing App #{app.Id}: {app.Title}");
                
                // 2. נעביר אותן לסטטוס "בטיפול" (3)
                _repository.UpdateStatus(app.Id, (int)ApplicationStatus.InProcess);
            }
            
            if (apps.Count == 0)
            {
                Console.WriteLine("No applications found.");
            }
        }
    }
}