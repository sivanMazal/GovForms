using Microsoft.EntityFrameworkCore;
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;

namespace GovForms.Engine.Data
{
    public class GovFormsDbContext : DbContext
    {
        public GovFormsDbContext(DbContextOptions<GovFormsDbContext> options) : base(options)
        {
        }

        // הגדרת הטבלאות ב-SQL (Mapping) [cite: 2025-12-30]
        public DbSet<Application> Applications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<ApplicationDocument> ApplicationDocuments { get; set; }
        public DbSet<ApplicationHistory> ApplicationHistory { get; set; }
        public DbSet<Role> Roles { get; set; }
public DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // הגדרת מפתח ראשי לטבלת הסטטוס (לפי מה שראינו ב-SQL שלך) [cite: 2025-12-30]
            modelBuilder.Entity<Status>().HasKey(s => s.StatusID);
            
            // הגדרת מפתח ראשי לטבלת המשתמשים
            modelBuilder.Entity<User>().HasKey(u => u.UserID);

            base.OnModelCreating(modelBuilder);
        }
    }
}