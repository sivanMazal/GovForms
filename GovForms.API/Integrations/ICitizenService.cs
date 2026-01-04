using System.Threading.Tasks;

namespace GovForms.API.Integrations
{
    // התשובה שנקבל ממשרד הפנים
    public class CitizenStatus
    {
        public bool CanSubmitRequest { get; set; } // האם מותר לו להגיש?
        public string RejectionReason { get; set; } // למה לא?
    }

    public interface ICitizenService
    {
        // הפונקציה מחזירה Task כי בעולם האמיתי זה לוקח זמן (פעולה אסינכרונית)
        Task<CitizenStatus> ValidateCitizenAsync(int userId);
    }
}