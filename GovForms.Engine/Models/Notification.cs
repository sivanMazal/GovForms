namespace GovForms.Engine.Models
{
    public class Notification
    {
        public int NotificationID { get; set; } // IDENTITY ב-SQL [cite: 2026-01-11]
        public int ApplicationID { get; set; }
        public int UserID { get; set; }
        public string MessageContent { get; set; } = string.Empty;
        public DateTime SentDate { get; set; } = DateTime.Now;
        public bool IsSent { get; set; } = true;
    }
}