using System.ComponentModel.DataAnnotations;

namespace Dtos;

public class StatusDtoForCreate
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage ="Step is required")]
    public int Step { get; set; }
    public StatusDtoForCreate(string name, int step)
    {
        Name = name;
        Step = step;
    }
}