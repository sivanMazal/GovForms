using System;
using GovForms.Engine.Services;

namespace GovForms.Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            // יצירת השירות בלבד - אין גישה ישירה לדאטה מכאן!
            var workflowService = new WorkflowService();

            Console.WriteLine("===============================================");
            Console.WriteLine("National Form Processing System - v1.0");
            Console.WriteLine("===============================================");

            try
            {
                // הפעלת התהליך הממשלתי
                workflowService.ProcessAllPendingApplications();
                Console.WriteLine("\n[SUCCESS] כל הבקשות עובדו בהצלחה.");
            }
            catch (Exception ex)
            {
                // טיפול בשגיאות ברמת המערכת
                Console.WriteLine($"[ERROR] תקלה קריטית במערכת: {ex.Message}");
            }

            Console.WriteLine("לחצי על מקש לסגירת המערכת...");
            Console.ReadKey();
        }
    }
}