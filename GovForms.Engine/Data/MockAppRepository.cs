using System;
using System.Collections.Generic;
using System.Linq;
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;
using GovForms.Engine.Interfaces; // <--- השורה הזו חסרה לך!
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
                new Application { Id = 1, Title = "טופס בדיקה 1", StatusID = 1, Amount = 5000 },
                new Application { Id = 2, Title = "טופס בדיקה 2", StatusID = 1, Amount = 12000 }
            };
        }

        // התאמה לממשק אסינכרוני באמצעות Task.FromResult [cite: 2026-01-11]
        public Task<List<Application>> GetApplicationsByStatus(int statusId)
        {
            var list = _applications.Where(a => a.StatusID == statusId).ToList();
            return Task.FromResult(list);
        }

        public Task<Application?> GetApplicationById(int id)
        {
            var app = _applications.FirstOrDefault(a => a.Id == id);
            return Task.FromResult<Application?>(app);
        }

        // בתוך MockAppRepository.cs
public async Task UpdateStatus(int appId, int newStatusId, string remarks = "Mock Update")
{
    // ב-Mock אנחנו רק מדפיסים לקונסול כדי לראות שזה עובד בלי SQL
    Console.WriteLine($"[Mock SQL] Updating App {appId} to Status {newStatusId}. Remarks: {remarks}");
    await Task.CompletedTask;
}

        public Task LogHistory(ApplicationHistory history)
        {
            _historyLog.Add(history);
            Console.WriteLine($"[MOCK Audit] App #{history.ApplicationId} logged.");
            return Task.CompletedTask;
        }

        public Application AddApplication(Application app) 
        { 
            _applications.Add(app); 
            return app; 
        }
    }
}