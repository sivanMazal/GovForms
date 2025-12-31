using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums; // חובה כדי להכיר את ה-Enum
using System;
using System.Collections.Generic;
using System.Linq;

namespace GovForms.Engine.Data
{
    public class MockAppRepository : IAppRepository
    {
        // רשימה זמנית בזיכרון המותאמת למודל שלך
        private static List<Application> _fakeDb = new List<Application>
        {
            new Application 
            { 
                Id = 100, 
                Title = "בקשה להיתר בנייה", 
                Amount = 5000, 
                UserId = 10,
                Status = (ApplicationStatus)1, // המרה למצב "חדש"
                SubmissionDate = DateTime.Now 
            },
            new Application 
            { 
                Id = 101, 
                Title = "בקשה לרישיון עסק", 
                Amount = 12000, 
                UserId = 20,
                Status = (ApplicationStatus)1, 
                SubmissionDate = DateTime.Now 
            },
            new Application 
            { 
                Id = 102, 
                Title = "בקשה להנחה בארנונה", 
                Amount = 800, 
                UserId = 10,
                Status = (ApplicationStatus)1, 
                SubmissionDate = DateTime.Now 
            }
        };

        public List<Application> GetApplicationsByStatus(int statusId)
        {
            Console.WriteLine($"[MockDB] מביא נתונים מהזיכרון עבור סטטוס {statusId}...");
            // כאן אנחנו משווים את ה-Enum למספר שקיבלנו
            return _fakeDb.Where(a => (int)a.Status == statusId).ToList();
        }

        public void UpdateStatus(int appId, int newStatus)
        {
            var app = _fakeDb.FirstOrDefault(a => a.Id == appId);
            if (app != null)
            {
                // המרה מהמספר ל-Enum
                app.Status = (ApplicationStatus)newStatus;
                Console.WriteLine($"[MockDB] עודכן סטטוס לבקשה {appId} לסטטוס {newStatus}");
            }
        }
    }
}