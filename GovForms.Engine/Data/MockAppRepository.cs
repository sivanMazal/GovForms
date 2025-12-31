using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums; // חובה כדי להכיר את ה-Enum
using System;
using System.Collections.Generic;
using System.Linq;

namespace GovForms.Engine.Data
{
    public class MockAppRepository : IAppRepository
    {
        private static List<ApplicationHistory> _historyDb = new List<ApplicationHistory>();
        // רשימה זמנית בזיכרון המותאמת למודל שלך
private static List<Application> _fakeDb = new List<Application>
{
    // בקשה 100: סכום נמוך (5,000) - הפקיד אמור להצליח
    new Application 
    { 
        Id = 100, 
        Title = "בניית מרפסת", 
        Amount = 5000, // <--- הוספנו סכום!
        Type = ApplicationType.BuildingPermit,
        Status = (ApplicationStatus)1, 
        SubmissionDate = DateTime.Now,
        Documents = new List<string> { "Map.pdf", "Engineer_Plan.pdf" } 
    },
    
    // בקשה 101: סכום גבוה (12,000) - הפקיד אמור להיחסם! ⛔
    new Application 
    { 
        Id = 101, 
        Title = "פתיחת דוכן פלאפל", 
        Amount = 12000, // <--- הוספנו סכום גבוה!
        Type = ApplicationType.BusinessLicense,
        Status = (ApplicationStatus)1, 
        SubmissionDate = DateTime.Now,
        Documents = new List<string> { "RentalContract.pdf" } 
    },

    // בקשה 102: סכום נמוך (800) - הפקיד אמור להצליח
    new Application 
    { 
        Id = 102, 
        Title = "הנחת סטודנט", 
        Amount = 800, // <--- הוספנו סכום!
        Type = ApplicationType.TaxDiscount,
        Status = (ApplicationStatus)1, 
        SubmissionDate = DateTime.Now,
        Documents = new List<string> { "PaySlip_01.jpg" }
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
        public void LogHistory(ApplicationHistory history)
{
    // נותנים מזהה ייחודי לשורה ביומן
    history.Id = _historyDb.Count + 1;
    
    // שומרים בזיכרון
    _historyDb.Add(history);
    
    // מדפיסים למסך כדי שתראי שזה עובד
    Console.WriteLine($"   [AUDIT LOG] Saved: App {history.ApplicationId} | Action: {history.Action} | {history.Remarks}");
}
    }
    
}