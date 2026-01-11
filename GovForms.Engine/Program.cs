using System;
using System.Threading.Tasks;
using GovForms.Engine.Data; 
using GovForms.Engine.Models;
using GovForms.Engine.Services; 
using GovForms.Engine.Interfaces; // חובה כדי להכיר את INotificationService
using Microsoft.EntityFrameworkCore; 

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

// >>>>>>>>>> התיקון לשגיאה CS7036 נמצא כאן: <<<<<<<<<<
// אנחנו מעבירים גם את ה-repo וגם את ה-notificationService [cite: 2026-01-08]
var workflow = new WorkflowService(repo, notificationService);

// הפעלה אסינכרונית (חובה להוסיף await כי הפונקציה מחזירה Task) [cite: 2026-01-11]
await workflow.ApproveAsync(11); 

Console.WriteLine("Process Completed Successfully.");