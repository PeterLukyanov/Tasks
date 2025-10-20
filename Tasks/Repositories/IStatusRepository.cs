using Models;

namespace Repositorys;

public interface IStatusRepository
{
    IQueryable<Status> GetAll();
    Task AddAsync(Status status);
    Task AddRangeAsync(List<Status> statuses);
}