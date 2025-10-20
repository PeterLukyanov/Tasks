using Models;
using Dtos;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    private readonly TasksService _tasksService;

    public TasksController(TasksService tasksService)
    {
        _tasksService = tasksService;
    }

    [HttpGet("AllTasks")]
    public async Task<ActionResult<List<Task_>>> GetAllTasks()
    {
        var result = await _tasksService.GetAllTasks();
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            return NotFound(result.Error);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddTask([FromBody] TaskDtoForCreate dto)
    {
        var result = await _tasksService.AddNewTask(dto);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            return BadRequest(result.Error);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Task_>> GetTaskById(int id)
    {
        var result = await _tasksService.GetTaskWithId(id);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            return NotFound(result.Error);
        }
    }
}