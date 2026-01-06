using System.Collections.Generic;
using GovForms.Engine.Models;

namespace GovForms.Engine.Data
{
    public interface IAppRepository
    {
        List<Application> GetApplicationsByStatus(int statusId);
        void UpdateStatus(int appId, int newStatus);
        void LogHistory(ApplicationHistory history);
        
        // --- הוספנו את השורות החסרות האלו: ---
        Application AddApplication(Application app);
        Application GetApplicationById(int id);
    }
}