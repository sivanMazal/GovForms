using System.Collections.Generic;
using GovForms.Engine.Models;
using System.Threading.Tasks; 
// חובה עבור ה-Task! [cite: 2026-01-11]
namespace GovForms.Engine.Data
{
    public interface IAppRepository
    {
      Task UpdateStatus(int applicationId, int statusId);
    Task LogHistory(ApplicationHistory history);
Task<List<Application>> GetApplicationsByStatus(int statusId); // החזרת Task של רשימה [cite: 2026-01-11]
Task<Application?> GetApplicationById(int id);        
        // --- הוספנו את השורות החסרות האלו: ---
        Application AddApplication(Application app);
    }
}