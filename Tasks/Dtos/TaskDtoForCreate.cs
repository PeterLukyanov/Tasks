using System.ComponentModel.DataAnnotations;

namespace Dtos;

public class TaskDtoForCreate
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = null!;
    [Required(ErrorMessage ="Description is required")]
    public string Description { get; set; } = null!;

    public TaskDtoForCreate(string title, string description)
    {
        Title = title;
        Description = description;
    }


}