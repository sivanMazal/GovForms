using System.Collections.Generic;
using GovForms.Engine.Models;
using System.Threading.Tasks; 
// חובה עבור ה-Task! [cite: 2026-01-11]
namespace GovForms.Engine.Interfaces
{
    public interface IAppRepository
    {
        Task<Application?> GetApplicationById(int id);
        Task<List<Application>> GetApplicationsByStatus(int statusId);
        
        // עדכון החוזה: הוספת הפרמטר השלישי [cite: 2025-12-30]
        Task UpdateStatus(int appId, int newStatusId, string remarks = "עדכון סטטוס אוטומטי");
        
        Task LogHistory(ApplicationHistory history);
        Application AddApplication(Application app);
    }
}