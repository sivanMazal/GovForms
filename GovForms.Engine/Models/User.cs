using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 1. מודל משתמש (לפי התמונה של dbo.Users)
public class User
{
    [Key]
    public int UserID { get; set; }
public string TZ { get; set; } = null!;
public string Name { get; set; } = null!;
public string Email { get; set; } = null!;
    public int RoleID { get; set; }
}