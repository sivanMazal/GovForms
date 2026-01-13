using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;
using GovForms.Engine.Interfaces;

namespace GovForms.Engine.Data
{
    public class MockAppRepository : IAppRepository
    {
        // 1. מדמה שליפת טופס ספציפי (חשוב לבדיקת ה-Workflow)
        public Task<Application?> GetApplicationById(int id)
        {
            // אנחנו מחזירים טופס "דמה" כדי שה-ProcessApplication לא יקרוס
            var mockApp = new Application 
            { 
                Id = id, 
                Amount = 60000, // תוכלי לשנות כאן כדי לבדוק תרחישים שונים
                UserEmail = "test@mock.com",
                UserId = 1007
            };
            return Task.FromResult<Application?>(mockApp);
        }

        // 2. המימוש החשוב ביותר - עדכון הסטטוס (דרישה 7)
        public async Task UpdateStatus(int appId, int newStatusId, string remarks = "Mock Update")
        {
            // במקום לכתוב ל-SQL, אנחנו מדפיסים למסך (Console)
            // כך תדעי בעבודה שהלוגיקה שלך רצה נכון [cite: 2026-01-13]
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"[MOCK DB] SUCCESS: Application #{appId} Updated.");
            Console.WriteLine($"[MOCK DB] New Status ID: {newStatusId}");
            Console.WriteLine($"[MOCK DB] Audit Remarks: {remarks}");
            Console.WriteLine("--------------------------------------------------");
            
            await Task.CompletedTask;
        }

        // 3. מדמה רישום להיסטוריה
        public async Task LogHistory(ApplicationHistory history)
        {
            Console.WriteLine($"[MOCK HISTORY] {history.Action}: {history.Remarks}");
            await Task.CompletedTask;
        }

        public Task<List<Application>> GetApplicationsByStatus(int statusId)
        {
            return Task.FromResult(new List<Application>());
        }

        public Application AddApplication(Application app)
        {
            app.Id = new Random().Next(1000, 9999);
            Console.WriteLine($"[MOCK DB] New Application Created with ID: {app.Id}");
            return app;
        }
    }
}