using System;
using GovForms.Engine.Data; // <--- חובה! כדי שיכיר את התיקייה Data
using GovForms.Engine.Models;
using GovForms.Engine.Services; // <--- השורה הזו קריטית!
using Microsoft.EntityFrameworkCore; // השורה שחסרה כדי לזהות את DbContextOptionsBuilder
// --- תחילת התוכנית ---

Console.WriteLine("===============================================");
Console.WriteLine("National Form Processing System - v1.0");
Console.WriteLine("===============================================");

// >>>>>>>> כאן נכנס הקוד החדש (במקום השורה הישנה של new AppRepository) <<<<<<<<

bool useMockData = true; // true = עבודה (בלי SQL), false = בית (עם SQL)
IAppRepository repo;     // המשתנה הריק שיחזיק את המידע

if (useMockData)
{
    Console.WriteLine("[System] Running in MOCK mode (No SQL Database)");
    repo = new MockAppRepository(); // טוען את הזיוף
}
else
{
    // 1. הגדרת מחרוזת החיבור (Connection String)
    string connectionString = "Server=MY-LAPTOP;Database=GovFormsDB;Trusted_Connection=True;TrustServerCertificate=True;";
    Console.WriteLine("[System] Running in SQL mode");

    // 2. יצירת ה-Options עבור ה-DbContext (ללא שימוש ב-Dependency Injection) [cite: 2026-01-08]
    var optionsBuilder = new DbContextOptionsBuilder<GovFormsDbContext>();
    optionsBuilder.UseSqlServer(connectionString);

    // 3. יצירת ה-Context והזרקתו ל-Repository
    var dbContext = new GovFormsDbContext(optionsBuilder.Options);
    repo = new AppRepository(dbContext); 
}

// >>>>>>>> סוף הקוד החדש <<<<<<<<


// מכאן הקוד המקורי שלך ממשיך כרגיל...
// הוא משתמש ב-repo שיצרנו למעלה, והוא לא יודע אם זה זיוף או אמיתי

// למשל:
var workflow = new WorkflowService(repo);
await workflow.RunAsync();