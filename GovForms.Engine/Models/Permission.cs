using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GovForms.Engine.Models
{
    public class Permission
    {
        [Key]
        public int PermissionID { get; set; }
        
        // שינוי השם מ-Name ל-PermissionName כדי להתאים ל-SQL שלך
        [Required]
        public string PermissionName { get; set; } = null!; 
        
        public string Description { get; set; } = null!;

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}