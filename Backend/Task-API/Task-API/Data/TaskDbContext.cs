using Microsoft.EntityFrameworkCore;
using Task_API.Models;
using Tasks = Task_API.Models.Tasks;

namespace Task_API.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Users> users { get; set; }
        public DbSet<Tasks> task { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Users>(entity =>
            {
                // Primary Key
                entity.HasKey(u => u.ID);

                // Username: Required, max length, and unique index
                entity.Property(u => u.Username)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.HasIndex(u => u.Username)
                      .IsUnique();

                // Password: Required
                entity.Property(u => u.PasswordHash)
                      .IsRequired()
                      .HasMaxLength(255);

                // ConfirmPassword: Do NOT create a column in the database for this
                entity.Ignore(u => u.ConfirmPasswordHash);

                // Roles: Required with a sensible max length
                entity.Property(u => u.Roles)
                      .IsRequired()
                      .HasMaxLength(100);
            });
            modelBuilder.Entity<Tasks>(entity =>
            {
                // Table name
                entity.ToTable("Tasks");

                // Primary Key
                entity.HasKey(e => e.ID);

                // Properties
                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.Description)
                      .HasMaxLength(1000);

                entity.Property(e => e.IsCompleted)
                      .HasMaxLength(50)
                      .HasDefaultValue("false");

                entity.Property(e => e.Priority)
                      .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                      .HasDefaultValueSql("GETDATE()"); 

                entity.Property(e => e.DueDate)
                      .IsRequired();

                entity.Property(e => e.Remark)
                      .IsRequired(false)
                      .HasMaxLength(500);

                entity.Property(e => e.Username)
                      .IsRequired(false)
                      .HasMaxLength(100);
            });
        

    }


}
}
