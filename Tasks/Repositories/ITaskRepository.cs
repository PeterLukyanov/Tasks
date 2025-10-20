using Models;

namespace Repositorys;

public interface ITaskRepository
{
    IQueryable<Task_> GetAll();
    Task AddAsync(Task_ task);
    
}