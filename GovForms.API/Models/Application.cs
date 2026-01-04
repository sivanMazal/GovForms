using System;
using System.Collections.Generic;
using GovForms.Engine.Models.Enums;

namespace GovForms.Engine.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime SubmissionDate { get; set; }
        
        public int UserId { get; set; } // מי הגיש
        
        // שימי לב: זה Enum ולא string
        public ApplicationType Type { get; set; } 
        
        public ApplicationStatus Status { get; set; }

        // הנה התיקון: הרשימה של המסמכים
        public List<ApplicationDocument> Documents { get; set; } = new List<ApplicationDocument>();
    }

    // הוספנו את המחלקה הזו כדי שהשגיאה תיעלם
    public class ApplicationDocument
    {
        public string Name { get; set; }
        public DateTime UploadDate { get; set; }
    }
}