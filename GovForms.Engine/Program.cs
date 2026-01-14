using System;
using System.Threading.Tasks;
using GovForms.Engine.Data;
using GovForms.Engine.Models;
using GovForms.Engine.Services;
using GovForms.Engine.Interfaces; // חובה כדי להכיר את INotificationService
using Microsoft.EntityFrameworkCore;
using GovForms.Engine.Models.Enums; // ודאי שה-Using הזה קיים למעלה
Console.WriteLine("===============================================");
Console.WriteLine("National Form Processing System - v1.0");
Console.WriteLine("===============================================");

// 1. הגדרות בסיסיות
bool useMockData = true;
IAppRepository repo;
INotificationService notificationService; // השירות שהיה חסר! [cite: 2025-12-30]

if (useMockData)
{
    Console.WriteLine("[System] Running in MOCK mode");
    repo = new MockAppRepository();

    // יצירת מנוע התראות "מזויף" למשרד (את צריכה שיהיה לך קובץ כזה או פשוט להשתמש בקיים)
    notificationService = new NotificationService(null!);
}
else
{
    string connectionString = "Server=MY-LAPTOP;Database=GovFormsDB;Trusted_Connection=True;TrustServerCertificate=True;";
    Console.WriteLine("[System] Running in SQL mode");

    var optionsBuilder = new DbContextOptionsBuilder<GovFormsDbContext>();
    optionsBuilder.UseSqlServer(connectionString);
    var dbContext = new GovFormsDbContext(optionsBuilder.Options);

    // 2. יצירת השירותים האמיתיים עם ה-DbContext של הבית [cite: 2026-01-11]
    repo = new AppRepository(dbContext);
    notificationService = new NotificationService(dbContext);
}
// 1. יצירת השירות החדש (הסימולטור של משרד הפנים)
IExternalIntegrationService externalService = new PopulationRegistrySimulator();

// 2. יצירת ה-WorkflowService עם שלושת הפרמטרים [cite: 2026-01-13]
// שימי לב: הוספנו את externalService בסוף!
var workflow = new WorkflowService(repo, notificationService, externalService);

// 3. הפעלה (מכיוון שזה בבית, נבדוק טופס קיים מה-SQL)
await workflow.ApproveAsync(11, (int)UserRole.Reviewer);// >>>>>>>>>> התיקון לשגיאה CS7036 נמצא כאן: <<<<<<<<<<
// אנחנו מעבירים גם את ה-repo וגם את ה-notificationService [cite: 2026-01-08]

// הפעלה אסינכרונית (חובה להוסיף await כי הפונקציה מחזירה Task) [cite: 2026-01-11]

Console.WriteLine("Process Completed Successfully.");