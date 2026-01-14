using System.Collections.Generic;
using GovForms.Engine.Models;
using System.Threading.Tasks;
using GovForms.Engine.Models.Enums; // <--- השורה שחסרה לך!
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
        Task<UserRole?> GetUserRoleAsync(int userId);
    Task<bool> HasPermissionAsync(int userId, string permissionName);
    }
}