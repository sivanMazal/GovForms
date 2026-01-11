using System.Collections.Generic;
using GovForms.Engine.Interfaces;

namespace GovForms.Engine.Validators
{
    public class ValidatorFactory
    {
        // ההגדרה הפרטית שהייתה חסרה [cite: 2026-01-08]
        private readonly Dictionary<string, IValidator> _validators;

        public ValidatorFactory()
        {
            // אתחול המילון בתוך הבנאי
            _validators = new Dictionary<string, IValidator>();
        }

        public IValidator GetValidator(string type)
        {
            // בדיקה אם הולידטור קיים במילון
            if (_validators != null && _validators.ContainsKey(type))
            {
                return _validators[type];
            }
            
            // החזרת null בטוח למקרה שלא נמצא (מונע קריסה)
            return null!; 
        }
    }
}