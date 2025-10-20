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

    public async Task<Result<Task_>> GetTaskWithId(int id)
    {
        var isExistTask = await _unit.taskRepository.GetAll().FirstOrDefaultAsync(task => task.Id == id);
        if (isExistTask == null)
        {
            return Result.Failure<Task_>($"Task with this {id} id not exist");
        }
        else
        {
            return Result.Success(isExistTask);
        }
    }

    public async Task<Result<Task_>> UpdateStatus(TaskDtoForChangeStatus dto, int id)
    {
        var isExistTask = await _unit.taskRepository.GetAll().FirstOrDefaultAsync(task => task.Id == id);
        if (isExistTask == null)
        {
            return Result.Failure<Task_>($"Task with this {id} id not exist");
        }
        else
        {
            if ((isExistTask.Status == "Backlog" && dto.Status == "InWork")
                || (isExistTask.Status == "InWork" && dto.Status == "Testing")
                || (isExistTask.Status == "Testing" && dto.Status == "Done"))
            {
                isExistTask.ChangeStatus(dto.Status);
                await _unit.SaveChangesAsync();
                return Result.Success(isExistTask);
            }
            else
            {
                return Result.Failure<Task_>("Status, that you chose is not valid, you need to choose status, that more then previous on one step and you need to follow rules of escalation of status: Backlog-->InWork-->Testing-->Done");
            }
        }
    }
}