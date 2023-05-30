namespace ShiftManager.ApplicationTest.Handlers;

public class UpdateEmployeeCommandHandlerTests
{
    [Fact]
    public async Task Can_UpdateEmployeeCommandHandler_WorksProperly()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("4 Oct 2024"), Shift.Day);

        var command = new UpdateEmployeeCommand
        {
            EmployeeId = 1,
            FirstName = "Test1",
            LastName = "Test2",
        };

        var shiftRepoMock = new Mock<IShiftRepo>();
        var handler = new UpdateEmployeeCommandHandler(shiftRepoMock.Object);

        shiftRepoMock
            .Setup(s => s.GetEmployeeAsync(1))
            .ReturnsAsync(employee);

        shiftRepoMock.Setup(s => s.UpdateEmployeeAsync(employee));

        // Act.
        var isSuccess = await handler.Handle(command, CancellationToken.None);

        // Assert.
        Assert.Equal("Test1", employee.FirstName);
        Assert.Equal("Test2", employee.LastName);

        Assert.Equal(DateTime.Parse("1 Mar 2023"), employee.Schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Morning, employee.Schedules.ElementAt(0).Shift);

        Assert.Equal(DateTime.Parse("4 Oct 2024"), employee.Schedules.ElementAt(1).Date);
        Assert.Equal(Shift.Day, employee.Schedules.ElementAt(1).Shift);

        shiftRepoMock.Verify(v => v.GetEmployeeAsync(1), Times.Once);
        shiftRepoMock.Verify(v => v.UpdateEmployeeAsync(employee), Times.Once);
        shiftRepoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Can_UpdateEmployeeCommandHandler_ThrowException_IfGetEmployeeAsyncThrowException()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("4 Oct 2024"), Shift.Day);

        var command = new UpdateEmployeeCommand
        {
            EmployeeId = 1,
            FirstName = "Test1",
            LastName = "Test2",
        };

        var shiftRepoMock = new Mock<IShiftRepo>();
        var handler = new UpdateEmployeeCommandHandler(shiftRepoMock.Object);

        shiftRepoMock
            .Setup(s => s.GetEmployeeAsync(1))
            .Throws<InvalidOperationException>();

        shiftRepoMock.Setup(s => s.UpdateEmployeeAsync(employee));

        // Act.
        var testCode = () => handler.Handle(command, CancellationToken.None);

        // Assert.
        await Assert.ThrowsAsync<InvalidOperationException>(testCode);

        Assert.Equal("Jon", employee.FirstName);
        Assert.Equal("Doe", employee.LastName);

        Assert.Equal(DateTime.Parse("1 Mar 2023"), employee.Schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Morning, employee.Schedules.ElementAt(0).Shift);

        Assert.Equal(DateTime.Parse("4 Oct 2024"), employee.Schedules.ElementAt(1).Date);
        Assert.Equal(Shift.Day, employee.Schedules.ElementAt(1).Shift);

        shiftRepoMock.Verify(v => v.GetEmployeeAsync(1), Times.Once);
        shiftRepoMock.Verify(v => v.UpdateEmployeeAsync(employee), Times.Never);
        shiftRepoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Can_UpdateEmployeeCommandHandler_ThrowException_IfUpdateEmployeeAsyncThrowException()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("4 Oct 2024"), Shift.Day);

        var command = new UpdateEmployeeCommand
        {
            EmployeeId = 1,
            FirstName = "Test1",
            LastName = "Test2",
        };

        var shiftRepoMock = new Mock<IShiftRepo>();
        var handler = new UpdateEmployeeCommandHandler(shiftRepoMock.Object);

        shiftRepoMock
            .Setup(s => s.GetEmployeeAsync(1))
            .ReturnsAsync(employee);

        shiftRepoMock
            .Setup(s => s.UpdateEmployeeAsync(employee))
            .Throws<InvalidOperationException>();

        // Act.
        var testCode = () => handler.Handle(command, CancellationToken.None);

        // Assert.
        await Assert.ThrowsAsync<InvalidOperationException>(testCode);

        Assert.Equal("Test1", employee.FirstName);
        Assert.Equal("Test2", employee.LastName);

        Assert.Equal(DateTime.Parse("1 Mar 2023"), employee.Schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Morning, employee.Schedules.ElementAt(0).Shift);

        Assert.Equal(DateTime.Parse("4 Oct 2024"), employee.Schedules.ElementAt(1).Date);
        Assert.Equal(Shift.Day, employee.Schedules.ElementAt(1).Shift);

        shiftRepoMock.Verify(v => v.GetEmployeeAsync(1), Times.Once);
        shiftRepoMock.Verify(v => v.UpdateEmployeeAsync(employee), Times.Once);
        shiftRepoMock.VerifyNoOtherCalls();
    }
}
