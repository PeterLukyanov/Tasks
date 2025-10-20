using Dtos;
using Models;
using UoW;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class TasksService
{
    private readonly IUnitOfWork _unit;

    public TasksService(IUnitOfWork unit)
    {
        _unit = unit;
    }

    public async Task<Result<List<Task_>>> GetAllTasks()
    {
        var isEmptyOrTasks = await _unit.taskRepository.GetAll().AnyAsync();
        if (isEmptyOrTasks)
        {
            return Result.Success(await _unit.taskRepository.GetAll().ToListAsync());
        }
        else
        {
            return Result.Failure<List<Task_>>("There are no tasks now");
        }
    }

    
}