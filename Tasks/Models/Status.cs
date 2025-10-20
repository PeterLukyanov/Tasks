namespace Models;

public class Status
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public int Step { get; private set; }
    public Status(string name, int step)
    {
        Name = name;
        Step = step;
    }
    
}