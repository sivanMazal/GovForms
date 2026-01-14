using FluentValidation;
using GovForms.Engine.Models;

namespace GovForms.API.Validators
{
    public class ApplicationValidator : AbstractValidator<Application>
    {
        public ApplicationValidator()
        {
            // 1. כותרת הטופס
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("חובה להזין כותרת לטופס.")
                .Length(3, 100).WithMessage("כותרת הטופס חייבת להיות בין 3 ל-100 תווים.");

            // 2. אימייל המשתמש
            RuleFor(x => x.UserEmail)
                .NotEmpty().WithMessage("חובה להזין כתובת אימייל.")
                .EmailAddress().WithMessage("כתובת האימייל שהוזנה אינה תקינה.");

            // 3. סכום (Amount) - דרישה מסוג Senior [cite: 2025-12-30]
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("סכום הטופס חייב להיות גדול מ-0.")
                .LessThanOrEqualTo(1000000).WithMessage("לא ניתן להגיש טופס על סכום העולה על מיליון ש\"ח.");

            // 4. ולידציה על מסמכים (דרישה מס' 2)
            RuleFor(x => x.AttachedDocuments)
                .Must(docs => docs != null && docs.Any())
                .WithMessage("חובה לצרף לפחות מסמך אחד כדי להגיש את הבקשה.");
        }
    }
}