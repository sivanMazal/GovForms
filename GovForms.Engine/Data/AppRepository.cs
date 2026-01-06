using System;
using System.Collections.Generic;
// using System.Data.SqlClient; // כרגע בהערה
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;

namespace GovForms.Engine.Data
{
    public class AppRepository : IAppRepository
    {
       private readonly string _connectionString;

        // הוספת הבנאי הזה תפתור את השגיאה האדומה ב-Program.cs
        public AppRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Application> GetApplicationsByStatus(int statusId)
        {
            // קוד SQL קיים... (לא שינינו)
            return new List<Application>(); 
        }

        public void UpdateStatus(int appId, int newStatus)
        {
            // קוד SQL קיים...
        }

        public void LogHistory(ApplicationHistory history)
        {
            // קוד SQL קיים...
        }

        // --- הוספנו את המימושים החסרים כדי להשתיק את השגיאה ---
        
        public Application AddApplication(Application app)
        {
            // כרגע נזרוק שגיאה כי אנחנו לא בונים את ה-SQL המלא היום
            throw new NotImplementedException("SQL implementation coming soon!");
        }

        public Application GetApplicationById(int id)
        {
            throw new NotImplementedException("SQL implementation coming soon!");
        }
    }
}