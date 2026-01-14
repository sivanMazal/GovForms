using System.Threading.Tasks;

namespace GovForms.API.Integrations
{
    // התשובה שנקבל ממשרד הפנים
    public class CitizenStatus
    {
        public bool CanSubmitRequest { get; set; } // האם מותר לו להגיש?
        public string? RejectionReason { get; set; } // הסימן שאלה אומר: "מותר שיהיה null"    }
    }
    public interface ICitizenService
    {
        // הפונקציה מחזירה Task כי בעולם האמיתי זה לוקח זמן (פעולה אסינכרונית)
        Task<CitizenStatus> ValidateCitizenAsync(int userId);
    }
}