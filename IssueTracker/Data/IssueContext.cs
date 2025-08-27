using IssueTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Data
{
    public class IssueContext: DbContext
    {

        public IssueContext(DbContextOptions<IssueContext> options) : base(options) { }

        public DbSet<Issue> Issues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Issue>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description);
                entity.Property(e => e.Status).HasConversion<int>();
                entity.Property(e => e.Priority).HasConversion<int>();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // Seed data
            modelBuilder.Entity<Issue>().HasData(
                new Issue
                {
                    Id = 1,
                    Title = "Sample Bug Report",
                    Description = "This is a sample bug that needs to be fixed",
                    Status = IssueStatus.Open,
                    Priority = Priority.High,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Issue
                {
                    Id = 2,
                    Title = "Feature Request: Dark Mode",
                    Description = "Users are requesting a dark mode option",
                    Status = IssueStatus.InProgress,
                    Priority = Priority.Medium,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
