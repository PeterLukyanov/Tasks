using Models;
using Microsoft.EntityFrameworkCore;
namespace Db;

public class TasksDb: DbContext
{
    public TasksDb(DbContextOptions options) : base(options) { }
    public DbSet<Task_> Tasks { get; set; } = null!;
}