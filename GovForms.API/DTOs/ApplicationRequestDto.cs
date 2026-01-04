using System.ComponentModel.DataAnnotations; // הספרייה שנותנת לנו את החוקים (Required, Range)

namespace GovForms.API.DTOs
{
    // DTO = Data Transfer Object
    // זה האובייקט שמשמש רק להעברת מידע מהמשתמש אלינו
    public class ApplicationRequestDto
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required]
        [Range(0, 10000000, ErrorMessage = "Amount must be positive")]
        public decimal Amount { get; set; }

        [Required]
        public string Type { get; set; } // למשל: "Building", "Business"
    }
}