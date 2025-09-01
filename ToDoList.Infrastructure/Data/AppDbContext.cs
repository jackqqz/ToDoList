using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Models;

namespace ToDoList.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<ToDoListEntity> ToDoLists { get; set; }
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<CategoryEntity> ToDoCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<CategoryEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            modelBuilder.Entity<ToDoListEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.HasMany(e => e.Items)
                      .WithOne()
                      .HasForeignKey("ToDoListId")
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ToDoItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.IsCompleted);
                entity.Property(e => e.DueDate);
                entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasOne<ToDoListEntity>()
                      .WithMany(e => e.Items)
                      .HasForeignKey("ToDoListId")
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<CategoryEntity>()
                      .WithMany()
                      .HasForeignKey("CategoryId")
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
