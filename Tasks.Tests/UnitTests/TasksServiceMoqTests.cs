using Models;
using MockQueryable;
using Repositorys;
using Moq;
using UoW;
using Services;
using Dtos;

public class TasksServiceMoqTests
{
    [Fact]
    public async Task GetAllTasks_TasksExist_ShouldReturnList()
    {
        var existTasks = new List<Task_>
        {
            new Task_("Title", "Perfect description")
        };

        var existStatuses = new List<Status>();

        var (service, taskRepoMock, unitMock, statusRepoMock) = Create_Service_RepoMocks_UnitMock(existTasks, existStatuses);

        var result = await service.GetAllTasks();

        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task GetAllTasks_TasksNotExist_ShouldFail()
    {
        var existTasks = new List<Task_>
        {

        };

        var existStatuses = new List<Status>();

        var (service, taskRepoMock, unitMock, statusRepoMock) = Create_Service_RepoMocks_UnitMock(existTasks, existStatuses);

        var result = await service.GetAllTasks();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no tasks now", result.Error);
    }

    [Fact]
    public async Task AddNewTask_NameUniq_ShouldAdd()
    {
        var existTasks = new List<Task_>
        {
            new Task_("Title", "Perfect description")
        };

        var existStatuses = new List<Status>();

        var dto = new TaskDtoForCreate
        (
            "Uniq title",
            "Another perfect description"
        );

        var (service, taskRepoMock, unitMock, statusRepoMock) = Create_Service_RepoMocks_UnitMock(existTasks, existStatuses);

        var result = await service.AddNewTask(dto);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        taskRepoMock.Verify(r => r.AddAsync(It.IsAny<Task_>()), Times.Once);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddNewTask_NameNotUniq_ShouldFail()
    {
        var existTasks = new List<Task_>
        {
            new Task_("Title", "Perfect description")
        };

        var existStatuses = new List<Status>();

        var dto = new TaskDtoForCreate
        (
            "Title",
            "Another perfect description"
        );

        var (service, taskRepoMock, unitMock, statusRepoMock) = Create_Service_RepoMocks_UnitMock(existTasks, existStatuses);

        var result = await service.AddNewTask(dto);

        Assert.True(result.IsFailure);
        Assert.Equal("Task with same title exist", result.Error);
        taskRepoMock.Verify(r => r.AddAsync(It.IsAny<Task_>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task GetTaskWithId_TaskExist_ShouldShowTask()
    {
        var existTasks = new List<Task_>
        {
            new Task_("Title", "Perfect description")
        };

        var existStatuses = new List<Status>();

        var (service, taskRepoMock, unitMock, statusRepoMock) = Create_Service_RepoMocks_UnitMock(existTasks, existStatuses);

        var result = await service.GetTaskWithId(existTasks[0].Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task GetTaskWithId_TaskNotExist_ShouldFail()
    {
        var existTasks = new List<Task_>
        {

        };

        var existStatuses = new List<Status>();

        var (service, taskRepoMock, unitMock, statusRepoMock) = Create_Service_RepoMocks_UnitMock(existTasks, existStatuses);

        var result = await service.GetTaskWithId(1);

        Assert.True(result.IsFailure);
        Assert.Equal("Task with this 1 id not exist", result.Error);
    }

    [Fact]
    public async Task UpdateStatus_TaskExist_NextStatusIsRight_ShouldPass()
    {
        var existTasks = new List<Task_>
        {
            new Task_("Title", "Perfect description")
        };

        var existStatuses = new List<Status>
        {
            new Status("Backlog",1),
            new Status("InWork",2),
            new Status("Testing",3),
            new Status("Done",4)
        };

        var (service, taskRepoMock, unitMock, statusRepoMock) = Create_Service_RepoMocks_UnitMock(existTasks, existStatuses);

        var dto = new TaskDtoForChangeStatus("InWork");

        var result = await service.UpdateStatus(dto, existTasks[0].Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateStatus_TaskNotExist_ShouldFail()
    {
        var existTasks = new List<Task_>
        {
            new Task_("Title", "Perfect description")
        };

        var existStatuses = new List<Status>
        {
            new Status("Backlog",1),
            new Status("InWork",2),
            new Status("Testing",3),
            new Status("Done",4)
        };

        var (service, taskRepoMock, unitMock, statusRepoMock) = Create_Service_RepoMocks_UnitMock(existTasks, existStatuses);

        var dto = new TaskDtoForChangeStatus("InWork");

        var result = await service.UpdateStatus(dto, 9999);

        Assert.True(result.IsFailure);
        Assert.Equal("Task with this 9999 id not exist", result.Error);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateStatus_TaskExist_NextStatusIsWrong_ShouldFail()
    {
        var existTasks = new List<Task_>
        {
            new Task_("Title", "Perfect description")
        };

        var existStatuses = new List<Status>
        {
            new Status("Backlog",1),
            new Status("InWork",2),
            new Status("Testing",3),
            new Status("Done",4)
        };

        var (service, taskRepoMock, unitMock, statusRepoMock) = Create_Service_RepoMocks_UnitMock(existTasks, existStatuses);

        var dto = new TaskDtoForChangeStatus("Testing");

        var result = await service.UpdateStatus(dto, existTasks[0].Id);

        Assert.True(result.IsFailure);
        Assert.Equal("Status, that you chose is not valid, you need to choose status, that more then previous on one step and you need to follow rules of escalation of status: Backlog-->InWork-->Testing-->Done", result.Error);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }
    
    [Fact]
    public async Task UpdateStatus_TaskExist_NextStatusIsLastOne_ShouldFail()
    {
        var existTasks = new List<Task_>
        {
            new Task_("Title", "Perfect description")
        };

        existTasks[0].ChangeStatus("Done");

        var existStatuses = new List<Status>
        {
            new Status("Backlog",1),
            new Status("InWork",2),
            new Status("Testing",3),
            new Status("Done",4)
        };

        var (service, taskRepoMock, unitMock, statusRepoMock) = Create_Service_RepoMocks_UnitMock(existTasks, existStatuses);

        var dto = new TaskDtoForChangeStatus("Testing");

        var result = await service.UpdateStatus(dto, existTasks[0].Id);

        Assert.True(result.IsFailure);
        Assert.Equal($"Status {existTasks[0].Status} is the last one status", result.Error);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    private (TasksService service,
             Mock<ITaskRepository> taskRepoMock,
            Mock<IUnitOfWork> unitMock,
            Mock<IStatusRepository> statusRepoMock) Create_Service_RepoMocks_UnitMock(List<Task_> existTasks, List<Status> existStatuses)
    {
        var statusRepoMock = new Mock<IStatusRepository>();
        statusRepoMock.Setup(r => r.GetAll()).Returns(existStatuses.BuildMock().AsQueryable());

        var taskRepoMock = new Mock<ITaskRepository>();
        taskRepoMock.Setup(r => r.GetAll()).Returns(existTasks.BuildMock().AsQueryable());

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(u => u.statusRepository).Returns(statusRepoMock.Object);
        unitMock.Setup(u => u.taskRepository).Returns(taskRepoMock.Object);

        var service = new TasksService(unitMock.Object);
        return (service, taskRepoMock, unitMock, statusRepoMock);
    }
}