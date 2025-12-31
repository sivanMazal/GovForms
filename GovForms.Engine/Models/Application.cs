using System;
using GovForms.Engine.Models.Enums;

namespace GovForms.Engine.Models
{
    public class Application
    {
        public int Id { get; set; }
        
        // אתחול כדי למנוע את האזהרה הצהובה (Non-nullable property)
        public string Title { get; set; } = string.Empty; 
        
        public ApplicationStatus Status { get; set; } // מצוין! נשאר עם זה
        
        public int UserId { get; set; }
        
        public decimal Amount { get; set; }
        
        // הוספנו את זה כי המערכת צריכה לדעת מתי הוגשה הבקשה
        public DateTime SubmissionDate { get; set; } 
    }
}