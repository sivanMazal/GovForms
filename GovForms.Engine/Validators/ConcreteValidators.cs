using System.Linq;
using GovForms.Engine.Models;

namespace GovForms.Engine.Validators
{
    // מומחה להיתר בנייה
    public class BuildingPermitValidator : IValidator
    {
        public bool Validate(Application app)
        {
            if (app.Documents == null) return false;
            // דורש גם מפה וגם תוכנית
            return app.Documents.Any(d => d.Contains("Map")) && 
                   app.Documents.Any(d => d.Contains("Plan"));
        }
    }

    // מומחה לרישיון עסק
    public class BusinessLicenseValidator : IValidator
    {
        public bool Validate(Application app)
        {
            if (app.Documents == null) return false;
            // דורש אישור משטרה
            return app.Documents.Any(d => d.Contains("Police"));
        }
    }

    // מומחה להנחה בארנונה
    public class TaxDiscountValidator : IValidator
    {
        public bool Validate(Application app)
        {
            if (app.Documents == null) return false;
            // דורש תלוש שכר
            return app.Documents.Any(d => d.Contains("PaySlip"));
        }
    }
}