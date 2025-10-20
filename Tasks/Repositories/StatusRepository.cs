using Db;
using Models;

namespace Repositorys;

public class StatusRepository : IStatusRepository
{
    private readonly TasksDb _db;
    public StatusRepository(TasksDb db)
    {
        _db = db;
    }
    public IQueryable<Status> GetAll()
    {
        return _db.Statuses.AsQueryable();
    }

    public async Task AddAsync(Status status)
    {
        await _db.Statuses.AddAsync(status);
    }
    
    public async Task AddRangeAsync(List<Status> statuses)
    {
        await _db.Statuses.AddRangeAsync(statuses);
    }

}

