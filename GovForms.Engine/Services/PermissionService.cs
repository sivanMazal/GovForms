using Microsoft.EntityFrameworkCore;
using GovForms.Engine.Data;         // השורה הזו פותרת את השגיאה של ה-DbContext
using GovForms.Engine.Models;       // גישה למודלים
using GovForms.Engine.Models.Enums; 
using GovForms.Engine.Interfaces; // השורה הזו פותרת את השגיאה! [cite: 2026-01-08]// גישה ל-Enums
namespace GovForms.Engine.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly GovFormsDbContext _context;

        public PermissionService(GovFormsDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CanUserApprove(int userId)
        {
            // שליפת המשתמש מה-DB לפי ה-UserID שצילמת [cite: 2025-12-30]
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
            
            // בדיקה מקצועית: האם הוא 'Reviewer' (2) או 'Admin' (3)? [cite: 2025-12-30]
            if (user != null && (user.RoleID == 2 || user.RoleID == 3))
            {
                return true;
            }
            return false;
        }
    }
}