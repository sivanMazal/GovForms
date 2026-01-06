using System;
using System.Linq;
using GovForms.Engine.Models;

namespace GovForms.Engine.Validators
{
    // מומחה להיתר בנייה [cite: 2025-12-30]
    public class BuildingPermitValidator : IValidator
    {
        public bool Validate(Application app)
        {
            // בדיקה מקצועית: שימוש בשם השדה הנכון וחיפוש בתוך שם הקובץ [cite: 2025-12-30]
            if (app.AttachedDocuments == null || !app.AttachedDocuments.Any()) return false;

            // דורש גם מפה וגם תוכנית - שימוש ב-StringComparison למניעת טעויות [cite: 2025-12-30]
            bool hasMap = app.AttachedDocuments.Any(d => d.FileName.Contains("Map", StringComparison.OrdinalIgnoreCase));
            bool hasPlan = app.AttachedDocuments.Any(d => d.FileName.Contains("Plan", StringComparison.OrdinalIgnoreCase));

            return hasMap && hasPlan;
        }
    }

    // מומחה לרישיון עסק [cite: 2025-12-30]
    public class BusinessLicenseValidator : IValidator
    {
        public bool Validate(Application app)
        {
            if (app.AttachedDocuments == null) return false;

            // דורש אישור משטרה [cite: 2025-12-30]
            return app.AttachedDocuments.Any(d => d.FileName.Contains("Police", StringComparison.OrdinalIgnoreCase));
        }
    }

    // מומחה להנחה בארנונה [cite: 2025-12-30]
    public class TaxDiscountValidator : IValidator
    {
        public bool Validate(Application app)
        {
            if (app.AttachedDocuments == null) return false;

            // דורש תלוש שכר [cite: 2025-12-30]
            return app.AttachedDocuments.Any(d => d.FileName.Contains("PaySlip", StringComparison.OrdinalIgnoreCase));
        }
    }
}