namespace GovForms.Engine.Models
{
    public class Role
    {
        public int RoleID { get; set; } // מתאים בדיוק ל-SQL שלך [cite: 2025-12-30]
        public string RoleName { get; set; }= null!;
        public string Description { get; set; }= null!;
    }
}