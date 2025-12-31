using GovForms.Engine.Models;
using System.Collections.Generic;

namespace GovForms.Engine.Data
{
    // זה החוזה שלנו. כל מי שרוצה להיות "מנהל נתונים" חייב לדעת לעשות את הפעולות האלו
    public interface IAppRepository
    {
        List<Application> GetApplicationsByStatus(int statusId);
        void UpdateStatus(int appId, int newStatus);
    }
}