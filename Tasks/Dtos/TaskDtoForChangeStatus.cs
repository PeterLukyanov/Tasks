namespace Dtos;

public class TaskDtoForChangeStatus
{
    public string Status { get; set; } = null!;
    public TaskDtoForChangeStatus(string status)
    {
        Status = status;
    }
}