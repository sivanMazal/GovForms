using System;
using GovForms.Engine.Models.Enums;

namespace GovForms.Engine.Models
{
    public class ApplicationHistory
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }  // לאיזו בקשה זה שייך?
        public ApplicationStatus Status { get; set; } // מה היה הסטטוס?
        public string Action { get; set; }      // מה קרה? (למשל: "חסימת אבטחה")
        public string Remarks { get; set; }     // הערות מילוליות
        public int UserId { get; set; }         // מי ביצע את הפעולה
        public DateTime Date { get; set; } = DateTime.Now; // מתי זה קרה
    }
}