using GovForms.Engine.Interfaces; // עבור INotificationService [cite: 2025-12-30]
using GovForms.Engine.Data;       // עבור GovFormsDbContext [cite: 2026-01-08]
using GovForms.Engine.Models;
public class NotificationService : INotificationService
{
    private readonly GovFormsDbContext _context;

    public NotificationService(GovFormsDbContext context)
    {
        _context = context;
    }

    public async Task SendStatusUpdate(int applicationId, int userId, string message)
    {
        // 1. תמיד נדפיס ל-Console (יעבוד מצוין מחר במשרד) [cite: 2026-01-11]
        Console.WriteLine($"[NOTIFICATION] App: {applicationId}, User: {userId}, Msg: {message}");

        try
        {
            // 2. ניסיון שמירה ל-SQL (יעבוד בבית, ייכשל במשרד ללא שגיאה קריטית) [cite: 2026-01-11]
            var notification = new Notification
            {
                ApplicationID = applicationId,
                UserID = userId,
                MessageContent = message
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            // במשרד זה יגיע לכאן - אנחנו פשוט נדפיס אזהרה וזהו [cite: 2026-01-11]
            Console.WriteLine("Note: SQL Save skipped (No database connection).");
        }
    }
}