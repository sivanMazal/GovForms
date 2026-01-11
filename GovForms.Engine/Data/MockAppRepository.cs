using System;
using System.Collections.Generic;
using System.Linq;
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;

namespace GovForms.Engine.Data
{
    public class MockAppRepository : IAppRepository
    {
        private readonly List<Application> _applications;
        private readonly List<ApplicationHistory> _historyLog;

        public MockAppRepository()
        {
            _historyLog = new List<ApplicationHistory>();
            _applications = new List<Application>
            {
                new Application { 
                    Id = 1, 
                    Title = "היתר בנייה - תל אביב", 
                    // המרה ל-int עבור השדה החדש StatusID [cite: 2026-01-08]
                    StatusID = (int)ApplicationStatus.NotSubmitted, 
                    Type = ApplicationType.BuildingPermit, 
                    Amount = 15000, 
                    UserId = 555 
                },
                new Application { 
                    Id = 2, 
                    Title = "בקשת הנחה בארנונה", 
                    StatusID = (int)ApplicationStatus.NotSubmitted, 
                    Type = ApplicationType.TaxDiscount, 
                    Amount = 2000, 
                    UserId = 555 
                }
            };
        }

        public List<Application> GetApplicationsByStatus(int statusId)
        {
            // השוואה ישירה בין מספרים [cite: 2026-01-08]
            return _applications.Where(a => a.StatusID == statusId).ToList();
        }

        public void UpdateStatus(int appId, int newStatusId)
        {
            var app = _applications.FirstOrDefault(a => a.Id == appId);
            if (app != null)
            {
                app.StatusID = newStatusId; // עדכון השדה החדש [cite: 2026-01-08]
            }
        }

        public void LogHistory(ApplicationHistory history)
        {
            _historyLog.Add(history);
            // שימוש ב-(ApplicationStatus) כדי להדפיס את השם הטקסטואלי של הסטטוס [cite: 2026-01-08]
            Console.WriteLine($"[Audit] App #{history.ApplicationId} status: {(ApplicationStatus)history.Status}"); 
        }

        public Application GetApplicationById(int id) => _applications.FirstOrDefault(a => a.Id == id);
        public Application AddApplication(Application app) { _applications.Add(app); return app; }
    }
}