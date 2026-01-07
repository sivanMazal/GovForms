namespace GovForms.API.DTOs
{
    public class ApplicationRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        
        // השורה שחסרה לך:
        public int UserId { get; set; } 
    }
}