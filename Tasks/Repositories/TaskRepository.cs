using Db;
using Models;

namespace Repositorys;

public class TaskRepository : ITaskRepository
{
    private readonly TasksDb _db;

    public TaskRepository(TasksDb db)
    {
        _db = db;
    }

    public IQueryable<Task_> GetAll()
    {
        return _db.Tasks.AsQueryable();
    }

    public async Task AddAsync(Task_ task)
    {
        await _db.Tasks.AddAsync(task);
    }
}