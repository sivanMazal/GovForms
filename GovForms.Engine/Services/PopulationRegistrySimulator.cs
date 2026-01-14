using GovForms.Engine.Interfaces;

namespace GovForms.Engine.Services
{
    public class PopulationRegistrySimulator : IExternalIntegrationService
    {
        public async Task<bool> HasOutstandingDebtsAsync(string Email)
        {
            await Task.Delay(500); // דימוי זמן המתנה לפנייה לשרת חיצוני [cite: 2026-01-11]

            // סימולציה: אם המייל מכיל "debt", נחזיר שיש חוב
            return Email.Contains("debt", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<bool> IsIdentityVerifiedAsync(int userId)
        {
            await Task.Delay(300);
            return userId > 0; // פשוט מוודא שיש מזהה תקין
        }
    }
}