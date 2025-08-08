using DevSkill.Inventory.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Web.Data
{
    public class TrainingDbContext:DbContext
    {
        public DbSet<LogTable> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LogTable>(entity =>
            {
                entity.ToTable("Logs");
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.Level).IsRequired().HasMaxLength(128);
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.Exception).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Properties).HasColumnType("nvarchar(max)");
                entity.Property(e => e.MachineName).HasMaxLength(256);
                entity.Property(e => e.ThreadId).HasMaxLength(256);
            });
        }
    }
}
