namespace GovForms.Engine.Interfaces
{
    public interface INotificationService
    {
        // שליחת התראה למשתמש על שינוי סטטוס [cite: 2025-12-30]
Task SendStatusUpdate(int applicationId, int userId, string message);    }
}