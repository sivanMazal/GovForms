using GovForms.Engine.Models.Enums;

namespace GovForms.Engine.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
// בתוך Application.cs
public int StatusID { get; set; } // שימי לב ל-ID הגדול [cite: 2026-01-08]        
        // השדה שהיה חסר בשגיאה CS0117 [cite: 2026-01-08]
        public ApplicationType Type { get; set; } 
        
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; } = null!;
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
        public List<ApplicationDocument> AttachedDocuments { get; set; } = new();
    }
}