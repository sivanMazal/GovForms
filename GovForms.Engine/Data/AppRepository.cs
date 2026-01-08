using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;
using GovForms.Engine.Data;

namespace GovForms.Engine.Data
{
    public class AppRepository : IAppRepository
    {
        private readonly GovFormsDbContext _context;

        public AppRepository(GovFormsDbContext context)
        {
            _context = context;
        }

        public Application GetApplicationById(int id)
        {
            // שליפה מקצועית הכוללת גם את המסמכים המצורפים [cite: 2025-12-30]
            return _context.Applications
                .Include(a => a.AttachedDocuments)
                .FirstOrDefault(a => a.Id == id)!;
        }

        public List<Application> GetApplicationsByStatus(int statusId)
        {
            // שליפה מבוססת LINQ - הרבה יותר בטוח וקריא מ-SQL גולמי [cite: 2026-01-08]
            return _context.Applications
                .Where(a => a.StatusId == statusId)
                .ToList();
        }

        public void UpdateStatus(int appId, int newStatusId)
        {
            var app = _context.Applications.Find(appId);
            if (app != null)
            {
                app.StatusId = newStatusId;
                _context.SaveChanges(); // עדכון אוטומטי ב-SQL [cite: 2025-12-30]
            }
        }

        public void LogHistory(ApplicationHistory history)
        {
            // תיעוד במערכת ה-Audit (דרישה מס' 4) [cite: 2025-12-30]
            _context.ApplicationHistory.Add(history);
            _context.SaveChanges();
        }

        public Application AddApplication(Application app)
        {
            // הגדרת תאריך הגשה וסטטוס ראשוני [cite: 2025-12-30]
            app.SubmissionDate = DateTime.Now;
            
            _context.Applications.Add(app);
            _context.SaveChanges(); // כאן ה-ID נוצר אוטומטית על ידי ה-SQL [cite: 2026-01-08]
            
            return app;
        }
    }
}