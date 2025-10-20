namespace Dtos;

public class TaskDtoForCreate
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;

    public TaskDtoForCreate(string title, string description)
    {
        Title = title;
        Description = description;
    }


}