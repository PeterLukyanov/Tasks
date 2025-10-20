using Models;
using MockQueryable;
using Repositorys;
using Moq;
using UoW;
using Services;
using Dtos;

public class StatusServiceMoqTests
{

    [Fact]
    public async Task LoadDefaultStatuses_StatusesNotExist_ShouldCreate()
    {
        var existStatuses = new List<Status>
        {

        };

        var (service, unitMock, statusRepoMock) = Create_Service_RepoMock_UnitMock(existStatuses);

        await service.LoadDefaultStatuses();

        statusRepoMock.Verify(r => r.AddRangeAsync(It.IsAny<List<Status>>()), Times.Once);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task LoadDefaultStatuses_StatusesExist_ShouldSkip()
    {
        var existStatuses = new List<Status>
        {
            new Status("Backlog",1),
            new Status("InWork",2),
            new Status("Testing",3),
            new Status("Done",4)
        };

        var (service, unitMock, statusRepoMock) = Create_Service_RepoMock_UnitMock(existStatuses);

        await service.LoadDefaultStatuses();

        statusRepoMock.Verify(r => r.AddRangeAsync(It.IsAny<List<Status>>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task AddNewStatus_StatusUniq_ShouldPass()
    {
        var existStatuses = new List<Status>
        {
            new Status("Backlog",1),
            new Status("InWork",2),
            new Status("Testing",3),
            new Status("Done",4)
        };

        var (service, unitMock, statusRepoMock) = Create_Service_RepoMock_UnitMock(existStatuses);

        var dto = new StatusDtoForCreate("After Done", 5);

        var result = await service.AddNewStatus(dto);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        statusRepoMock.Verify(r => r.AddAsync(It.IsAny<Status>()), Times.Once);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddNewStatus_StatusNotUniq_ShouldFail()
    {
        var existStatuses = new List<Status>
        {
            new Status("Backlog",1),
            new Status("InWork",2),
            new Status("Testing",3),
            new Status("Done",4)
        };

        var (service, unitMock, statusRepoMock) = Create_Service_RepoMock_UnitMock(existStatuses);

        var dto = new StatusDtoForCreate("Done", 4);

        var result = await service.AddNewStatus(dto);

        Assert.True(result.IsFailure);
        Assert.Equal($"The {dto.Name.Trim()} type of status exist, try another",result.Error);
        statusRepoMock.Verify(r => r.AddAsync(It.IsAny<Status>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    private (StatusService service,
            Mock<IUnitOfWork> unitMock,
            Mock<IStatusRepository> statusRepoMock )
            Create_Service_RepoMock_UnitMock(List<Status> existStatuses)

    {
        var statusRepoMock = new Mock<IStatusRepository>();
        statusRepoMock.Setup(r => r.GetAll()).Returns(existStatuses.BuildMock().AsQueryable());

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(u => u.statusRepository).Returns(statusRepoMock.Object);

        var service = new StatusService(unitMock.Object);
        return (service, unitMock, statusRepoMock);
    }
}