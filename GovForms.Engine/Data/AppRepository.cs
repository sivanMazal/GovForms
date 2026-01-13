using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; // חובה עבור Task [cite: 2026-01-11]
using Microsoft.EntityFrameworkCore;
using GovForms.Engine.Models;
using GovForms.Engine.Data;
using GovForms.Engine.Models.Enums;
using GovForms.Engine.Interfaces; // <--- השורה הזו חובה! [cite: 2026-01-08]
namespace GovForms.Engine.Data
{
    public class AppRepository : IAppRepository
    {
        private readonly GovFormsDbContext _context;

        public AppRepository(GovFormsDbContext context)
        {
            _context = context;
        }

        // שינוי ל-async Task<Application?> כדי להתאים לממשק [cite: 2026-01-08]
        public async Task<Application?> GetApplicationById(int id)
        {
            return await _context.Applications
                .Include(a => a.AttachedDocuments)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Application>> GetApplicationsByStatus(int statusId)
        {
            return await _context.Applications
                .Where(a => a.StatusID == statusId)
                .ToListAsync(); // שליפה אסינכרונית [cite: 2026-01-11]
        }

     public async Task UpdateStatus(int appId, int newStatusId, string remarks = "עדכון סטטוס אוטומטי")
{
    var app = await _context.Applications.FindAsync(appId);
    if (app != null)
    {
        // 1. עדכון הסטטוס בטבלה הראשית
        app.StatusID = newStatusId;

        // 2. יצירת שורת היסטוריה אוטומטית - דרישה מס' 4 [cite: 2025-12-30]
        var history = new ApplicationHistory
        {
            ApplicationId = appId,
            Status = (ApplicationStatus)newStatusId,
            Action = "Status Change",
            Remarks = remarks,
            Timestamp = DateTime.Now,
            UserId = app.UserId // מקשרים למשתמש של הבקשה
        };

        _context.ApplicationHistory.Add(history);

        // 3. שמירה של הכל בטרנזקציה אחת (הכל או כלום) [cite: 2026-01-11]
        await _context.SaveChangesAsync();
    }
}

        public async Task LogHistory(ApplicationHistory history)
        {
            _context.ApplicationHistory.Add(history);
            await _context.SaveChangesAsync();
        }

        public Application AddApplication(Application app)
        {
            app.SubmissionDate = DateTime.Now;
            _context.Applications.Add(app);
            _context.SaveChanges(); // כאן השארנו סינכרוני לפי הממשק שלך [cite: 2025-12-31]
            return app;
        }
    }
}