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

    public async Task<Result<Task_>> AddNewTask(TaskDtoForCreate dto)
    {
        var isExistSameTask = await _unit.taskRepository.GetAll().FirstOrDefaultAsync(task => task.Title.Trim().ToLower() == dto.Title.Trim().ToLower());
        if (isExistSameTask == null)
        {
            var task = new Task_(dto.Title, dto.Description);
            await _unit.taskRepository.AddAsync(task);
            await _unit.SaveChangesAsync();
            return Result.Success(task);
        }
        else
        {
            return Result.Failure<Task_>("Task with same title exist");
        }
    }
}