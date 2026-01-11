using GovForms.Engine.Data;
using GovForms.Engine.Interfaces;
using Microsoft.EntityFrameworkCore;

public class PermissionService : IPermissionService
{
    private readonly GovFormsDbContext _context;

    public PermissionService(GovFormsDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CheckRole(int userId, int roleId)
    {
        // בדיקה ב-SQL האם למשתמש יש את ה-RoleID המבוקש [cite: 2025-12-30]
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
        return user != null && user.RoleID == roleId;
    }

    public async Task<bool> CanUserApprove(int userId)
    {
        // שימוש בפונקציה הכללית עם מזהה הבודק (Reviewer = 1005) [cite: 2026-01-11]
        return await CheckRole(userId, 1005);
    }
}