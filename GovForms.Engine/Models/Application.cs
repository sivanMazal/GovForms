using GovForms.Engine.Models.Enums; // ודאי שהשורה הזו קיימת

namespace GovForms.Engine.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int StatusId { get; set; } 
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; } 
        public DateTime SubmissionDate { get; set; }
        
        // שינוי הטיפוסים ל-Enum פותר את שגיאות CS0029 ו-CS1503
        public ApplicationStatus Status { get; set; } 
        public ApplicationType Type { get; set; }     
        
        public List<ApplicationDocument> AttachedDocuments { get; set; } = new List<ApplicationDocument>();
    }
}