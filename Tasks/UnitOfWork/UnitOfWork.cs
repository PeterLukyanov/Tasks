using Repositorys;
using Db;

namespace UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly TasksDb _db;
    public ITaskRepository taskRepository { get; }

    public UnitOfWork(TasksDb db, ITaskRepository _taskRepository)
    {
        _db = db;
        taskRepository = _taskRepository;
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}