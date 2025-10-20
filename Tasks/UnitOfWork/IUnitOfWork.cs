using Repositorys;

namespace UoW;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
    ITaskRepository taskRepository{ get; }
}