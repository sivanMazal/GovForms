using System.Collections.Generic;

namespace GovForms.Engine.Models
{
    public class Role
    {
        public int RoleID { get; set; } 
        public string RoleName { get; set; } = null!;
        public string Description { get; set; } = null!;

        // התוספת הקריטית: רשימת ההרשאות המשויכות לתפקיד זה
        // זהו קשר של "רבים לרבים" (Many-to-Many) [cite: 2025-12-30]
        public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}