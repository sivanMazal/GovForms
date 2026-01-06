namespace GovForms.Engine.Models
{
    public class ApplicationHistory
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string Action { get; set; }
        public string Status { get; set; } // נשמר כ-string לתיעוד היסטורי
        public int UserId { get; set; } // מזהה המשתמש שביצע את הפעולה [cite: 2025-12-30]
        public DateTime Timestamp { get; set; } = DateTime.Now; // פותר את שגיאות ה-Build
        public string Remarks { get; set; }
    }
}