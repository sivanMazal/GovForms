namespace GovForms.Engine.Models
{
    public class ApplicationDocument
    {
        public int Id { get; set; } // מזהה המסמך [cite: 2025-12-30]
        public string FileName { get; set; } // שם הקובץ [cite: 2025-12-30]
        public string FileType { get; set; } // סוג הקובץ [cite: 2025-12-30]
        public byte[] Content { get; set; } // התוכן של הקובץ [cite: 2025-12-30]
    }
}