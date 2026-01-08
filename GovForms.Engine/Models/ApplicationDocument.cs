using GovForms.Engine.Models.Enums;
namespace GovForms.Engine.Models
{
    public class ApplicationDocument
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string FileName { get; set; }= null!;
        public string FilePath { get; set; }= null!;
        
        // התאמה לצילום המסך שלך: UploadedAt
        public DateTime UploadedAt { get; set; } = DateTime.Now; 
    }
}