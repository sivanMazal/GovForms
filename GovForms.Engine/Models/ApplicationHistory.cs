using System.ComponentModel.DataAnnotations.Schema;
using GovForms.Engine.Models.Enums;

namespace GovForms.Engine.Models
{
    public class ApplicationHistory
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int UserId { get; set; }
        
        // התיקון: אנחנו אומרים ל-EF שהשדה הזה מקושר לעמודה שנקראת 'Status' ב-SQL [cite: 2026-01-08]
        [Column("Status")] 
        public ApplicationStatus Status { get; set; }
        
        public string Action { get; set; } = null!;
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Remarks { get; set; } = null!;
    }
}