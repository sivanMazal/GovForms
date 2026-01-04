using System.Linq; // <--- חובה בשביל הפונקציה Any
using GovForms.Engine.Models;

namespace GovForms.Engine.Validators
{
    // --- מומחה להיתרי בנייה ---
    public class BuildingPermitValidator : IValidator
    {
        public bool Validate(Application app)
        {
            // בדיקה חדשה: האם קיים מסמך שהשם שלו הוא "Map"?
            bool hasMap = app.Documents.Any(d => d.Name == "Map");
            
            // בדיקה חדשה: האם קיים מסמך שהשם שלו הוא "ArchitectPlan"?
            bool hasPlan = app.Documents.Any(d => d.Name == "ArchitectPlan");

            return hasMap && hasPlan;
        }
    }

    // --- מומחה לרישיון עסק ---
    public class BusinessLicenseValidator : IValidator
    {
        public bool Validate(Application app)
        {
            // האם קיים מסמך "BusinessPlan"?
            if (!app.Documents.Any(d => d.Name == "BusinessPlan"))
            {
                return false;
            }

            return true;
        }
    }

    // --- מומחה להחזרי מס ---
    public class TaxRefundValidator : IValidator
    {
        public bool Validate(Application app)
        {
            // האם קיים תלוש משכורת?
            return app.Documents.Any(d => d.Name == "SalarySlip");
        }
    }
}