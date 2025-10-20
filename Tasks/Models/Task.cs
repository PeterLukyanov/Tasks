namespace Models;

public class Task_
{
    public int Id { get; private set; }
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Status { get; private set; } = "Backlog";
    public Task_(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public void ChangeStatus(string newStatus)
    {
        Status = newStatus;
    }
}