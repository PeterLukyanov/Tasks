using Microsoft.AspNetCore.Mvc;
using Services;
using Dtos;
using Models;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class StatusController : ControllerBase
{

    private readonly StatusService _statusService;

    public StatusController(StatusService statusService)
    {
        _statusService = statusService;
    }

    [HttpPost]
    public async Task<IActionResult> AddNewStatus([FromBody] StatusDtoForCreate dto)
    {
        var result = await _statusService.AddNewStatus(dto);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            return BadRequest(result.Error);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<Status>>> GetAll()
    {
        var result = await _statusService.GetAll();
        return Ok(result.Value);
    }
}