using System;
using System.Collections.Generic;
using System.Linq; // <--- חשוב לשימוש ב-FirstOrDefault
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;

namespace GovForms.Engine.Data
{
    public class MockAppRepository : IAppRepository
    {
        // רשימה בזיכרון שמדמה דאטה-בייס
        private static List<Application> _fakeDb = new List<Application>
        {
            new Application 
            { 
                Id = 100, 
                UserId = 999, 
                Title = "Building Permit", 
                Amount = 500, 
                Status = ApplicationStatus.NotSubmitted,
                Type = ApplicationType.BuildingPermit 
            },
            new Application 
            { 
                Id = 101, 
                UserId = 555, 
                Title = "Business License", 
                Amount = 12000, 
                Status = ApplicationStatus.NotSubmitted,
                Type = ApplicationType.BusinessLicense 
            }
        };

        public List<Application> GetApplicationsByStatus(int statusId)
        {
            return _fakeDb.Where(a => (int)a.Status == statusId).ToList();
        }

        public void UpdateStatus(int appId, int newStatus)
        {
            var app = _fakeDb.FirstOrDefault(a => a.Id == appId);
            if (app != null)
            {
                app.Status = (ApplicationStatus)newStatus;
            }
        }

        public void LogHistory(ApplicationHistory history)
        {
            Console.WriteLine($"[AUDIT LOG] Saved: App {history.ApplicationId} | Status: {history.Status} | Action: {history.Action}");
        }

        // --- הוספנו את שתי הפונקציות החסרות כאן: ---

        public Application AddApplication(Application app)
        {
            // סימולציה של Auto-Increment (נותנים ID חדש)
            int newId = _fakeDb.Any() ? _fakeDb.Max(a => a.Id) + 1 : 1;
            app.Id = newId;
            
            _fakeDb.Add(app);
            return app;
        }

        public Application GetApplicationById(int id)
        {
            return _fakeDb.FirstOrDefault(a => a.Id == id);
        }
    }
}