using System;
using System.Collections.Generic;
using System.Linq;
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;

namespace GovForms.Engine.Data
{
    public class MockAppRepository : IAppRepository
    {
        // פותר את שגיאות CS0103 - הגדרת הרשימות הפנימיות
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
                    Status = ApplicationStatus.NotSubmitted, // Enum תקין
                    Type = ApplicationType.BuildingPermit, 
                    Amount = 15000, 
                    UserId = 555 
                },
                new Application { 
                    Id = 2, 
                    Title = "בקשת הנחה בארנונה", 
                    Status = ApplicationStatus.NotSubmitted, 
                    Type = ApplicationType.TaxDiscount, 
                    Amount = 2000, 
                    UserId = 555 
                }
            };
        }

       public List<Application> GetApplicationsByStatus(int statusId)
{
    // המרה מפורשת (Casting) ל-int פותרת את שגיאת CS0030
    return _applications.Where(a => (int)a.Status == statusId).ToList();
}

        public void UpdateStatus(int appId, int newStatusId)
        {
            var app = _applications.FirstOrDefault(a => a.Id == appId);
            if (app != null)
            {
                // המרה מ-int בחזרה ל-Enum
                app.Status = (ApplicationStatus)newStatusId;
            }
        }

       public void LogHistory(ApplicationHistory history)
{
    _historyLog.Add(history);
    // המרה ל-string לצורך הדפסה או שמירה בטבלת טקסט
    Console.WriteLine($"[Audit] App #{history.ApplicationId} status: {history.Status}"); 
}

        public Application GetApplicationById(int id) => _applications.FirstOrDefault(a => a.Id == id);
        public Application AddApplication(Application app) { _applications.Add(app); return app; }
    }
}