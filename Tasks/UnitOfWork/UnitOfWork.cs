using Repositorys;
using Db;

namespace UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly TasksDb _db;
    public ITaskRepository taskRepository { get; }
    public IStatusRepository statusRepository{get;}

    public UnitOfWork(TasksDb db, ITaskRepository _taskRepository, IStatusRepository _statusRepository)
    {
        _db = db;
        taskRepository = _taskRepository;
        statusRepository = _statusRepository;
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}