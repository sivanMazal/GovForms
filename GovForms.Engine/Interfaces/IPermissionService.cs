namespace GovForms.Engine.Interfaces
{
    public interface IPermissionService
    {
        // הפונקציה החדשה שתאפשר לנו לבדוק כל תפקיד מול ה-SQL [cite: 2025-12-30]
        Task<bool> CheckRole(int userId, int roleId);
        
        // אפשר להשאיר את זו כקיצור דרך לאישור (אופציונלי)
        Task<bool> CanUserApprove(int userId);
    }
}