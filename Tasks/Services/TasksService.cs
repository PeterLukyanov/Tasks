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
            var statusesList = await _unit.statusRepository.GetAll().ToListAsync();
            if (statusesList != null)
            {
                int currentTaskStep = statusesList.Find(status => status.Name == isExistTask.Status)!.Step;

                if (isExistTask.Status == statusesList.First(status => status.Step == statusesList.Max(status=>status.Step)).Name)
                {
                    return Result.Failure<Task_>($"Status {isExistTask.Status} is the last one status");
                }
                else if (currentTaskStep+1 == statusesList.First(status=>status.Name==dto.Status).Step)
                {
                    isExistTask.ChangeStatus(dto.Status);
                    await _unit.SaveChangesAsync();
                    return Result.Success(isExistTask);
                }

            }

            return Result.Failure<Task_>("Status, that you chose is not valid, you need to choose status, that more then previous on one step and you need to follow rules of escalation of status: Backlog-->InWork-->Testing-->Done");

        }
    }
}