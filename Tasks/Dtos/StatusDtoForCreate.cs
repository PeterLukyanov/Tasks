namespace Dtos;

public class StatusDtoForCreate
{
    public string Name { get; set; } = null!;
    public int Step { get; set; }
    public StatusDtoForCreate(string name, int step)
    {
        Name = name;
        Step = step;
    }
}