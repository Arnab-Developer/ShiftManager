namespace ShiftManager.ApplicationTest.Handlers;

public class CreateEmployeeCommandHandlerTests
{
    [Fact]
    public async Task Can_CreateEmployeeCommandHandler_WorksProperly()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("4 Oct 2024"), Shift.Day);

        var command = new CreateEmployeeCommand { FirstName = "Jon", LastName = "Doe" };
        var shiftRepoMock = new Mock<IShiftRepo>();
        var handler = new CreateEmployeeCommandHandler(shiftRepoMock.Object);

        shiftRepoMock
            .Setup(s => s.CreateEmployeeAsync(It.IsAny<Employee>()))
            .Returns(Task.FromResult(true));

        // Act.
        var isSuccess = await handler.Handle(command, CancellationToken.None);

        // Assert.
        Assert.True(isSuccess);
        shiftRepoMock.Verify(v => v.CreateEmployeeAsync(It.IsAny<Employee>()), Times.Once);
        shiftRepoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Can_CreateEmployeeCommandHandler_ThrowException()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("4 Oct 2024"), Shift.Day);

        var command = new CreateEmployeeCommand { FirstName = "Jon", LastName = "Doe" };
        var shiftRepoMock = new Mock<IShiftRepo>();
        var handler = new CreateEmployeeCommandHandler(shiftRepoMock.Object);

        shiftRepoMock
            .Setup(s => s.CreateEmployeeAsync(It.IsAny<Employee>()))
            .Throws<InvalidOperationException>();

        // Act.
        var testCode = () => handler.Handle(command, CancellationToken.None);

        // Assert.
        await Assert.ThrowsAsync<InvalidOperationException>(testCode);
        shiftRepoMock.Verify(v => v.CreateEmployeeAsync(It.IsAny<Employee>()), Times.Once);
    }
}
