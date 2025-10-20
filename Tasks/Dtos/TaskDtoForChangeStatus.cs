using System.ComponentModel.DataAnnotations;

namespace Dtos;

public class TaskDtoForChangeStatus
{
    [Required(ErrorMessage ="Status is required")]
    public string Status { get; set; } = null!;
    public TaskDtoForChangeStatus(string status)
    {
        Status = status;
    }
}