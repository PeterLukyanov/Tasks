using Microsoft.EntityFrameworkCore;
using UoW;
using CSharpFunctionalExtensions;
using Models;
using Dtos;

namespace Services;

public class StatusService
{
    private readonly IUnitOfWork _unit;

    public StatusService(IUnitOfWork unit)
    {
        _unit = unit;
    }

    public async Task LoadDefaultStatuses()
    {
        var isExistStatuses = await _unit.statusRepository.GetAll().AnyAsync();
        if (isExistStatuses)
        {
            return;
        }
        else
        {
            var statuses = new List<Status>
            {
                new Status("Backlog",1),
                new Status("InWork",2),
                new Status("Testing",3),
                new Status("Done",4)
            };
            await _unit.statusRepository.AddRangeAsync(statuses);
            await _unit.SaveChangesAsync();
        }
    }
    public async Task<Result<Status>> AddNewStatus(StatusDtoForCreate dto)
    {
        var isExistStatus = await _unit.statusRepository.GetAll().FirstOrDefaultAsync(status => status.Name.ToLower() == dto.Name.Trim().ToLower());
        if (isExistStatus == null)
        {
            var newStatus = new Status(dto.Name,dto.Step); 
            await _unit.statusRepository.AddAsync(newStatus);
            await _unit.SaveChangesAsync();
            return Result.Success(newStatus);
        }
        else
        {
            return Result.Failure<Status>($"The {dto.Name.Trim()} type of status exist, try another");
        }
    }

    public async Task<Result<List<Status>>> GetAll()
    {

        return Result.Success<List<Status>>(await _unit.statusRepository.GetAll().ToListAsync());
    }
}