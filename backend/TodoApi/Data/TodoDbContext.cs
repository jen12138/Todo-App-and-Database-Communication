using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> TaskItems { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<TaskItem>().HasData(
        new TaskItem
        {
            Id = 1,
            Title = "Team Standup",
            Description = "Daily sync at 10:30 AM",
            IsCompleted = false,
            CreatedAt = new DateTime(2026, 3, 16)
        },
        new TaskItem
        {
            Id = 2,
            Title = "Grocery Run",
            Description = "Pick up veggies",
            IsCompleted = true,
            CreatedAt = new DateTime(2026, 3, 16)
        }
    );
}

}
