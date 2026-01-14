using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GovForms.Engine.Models; // ודאי שה-Namespace נכון

public class User
{
    [Key]
    public int UserID { get; set; }
    
    public string TZ { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;

    // 1. זהו המפתח הזר (המספר ב-SQL)
    public int RoleID { get; set; }

    // 2. זוהי תוספת הקסם: Navigation Property
    // היא מאפשרת לנו לכתוב user.Role.Permissions בקוד [cite: 2026-01-11]
    [ForeignKey("RoleID")]
    public virtual Role Role { get; set; } = null!; 
}