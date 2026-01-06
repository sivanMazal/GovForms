using System;
using GovForms.Engine.Data; // <--- חובה! כדי שיכיר את התיקייה Data
using GovForms.Engine.Models;
using GovForms.Engine.Services; // <--- השורה הזו קריטית!
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
    // הגדרות למחשב בבית
    string connectionString = "Server=MY-LAPTOP;Database=GovFormsDB;Trusted_Connection=True;TrustServerCertificate=True;";
    repo = new AppRepository(connectionString); 
}

// >>>>>>>> סוף הקוד החדש <<<<<<<<


// מכאן הקוד המקורי שלך ממשיך כרגיל...
// הוא משתמש ב-repo שיצרנו למעלה, והוא לא יודע אם זה זיוף או אמיתי

// למשל:
var workflow = new WorkflowService(repo);
await workflow.RunAsync();