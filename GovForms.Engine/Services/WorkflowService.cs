using GovForms.Engine.Data;
using GovForms.Engine.Models;
using System.Collections.Generic;
using GovForms.Engine.Models.Enums;

namespace GovForms.Engine.Services
{
    public class WorkflowService
    {
        // יצירת הקשר לדאטה בתוך הסרוויס - הגנה על הגישה למסד הנתונים
        private readonly AppRepository _repo = new AppRepository();

        public void ProcessAllPendingApplications()
        {
            // השכבה הזו שולטת בתהליך
            List<Application> apps = _repo.GetInProcessApplications();

            foreach (var app in apps)
            {
                // כאן נכניס בעתיד את הבדיקה האמיתית בטבלת Documents
                bool hasDocs = true;

                // קבלת החלטה לוגית
                var nextStatus = CalculateNextStatus(app, hasDocs);

                // עדכון הנתונים מתבצע רק דרך הלוגיקה של הסרוויס
                _repo.UpdateApplicationStatus(app.Id, nextStatus, "עובד אוטומטית עיי השירות הממשלתי", 1);
            }
        }

        private ApplicationStatus CalculateNextStatus(Application app, bool hasDocs)
        {
            if (!hasDocs) return ApplicationStatus.ReturnedForDocs;
            if (app.Amount > 10000) return ApplicationStatus.InProcess; // סכום גבוה נשאר לבדיקה אנושית

            return ApplicationStatus.Handled;
        }
    }
}