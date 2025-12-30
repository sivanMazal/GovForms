
using GovForms.Engine.Models.Enums;

namespace GovForms.Engine.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ApplicationStatus Status { get; set; } // שימוש ב-Enum במקום מספר
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}