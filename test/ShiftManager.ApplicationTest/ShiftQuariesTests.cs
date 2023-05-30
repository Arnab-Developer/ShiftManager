using ShiftManager.Application;

namespace ShiftManager.ApplicationTest;

public class ShiftQuariesTests
{
    [Fact]
    public async Task Can_GetSchedulesAsync_ReturnSchedules_WithEmpName()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("4 Oct 2024"), Shift.Day);

        var shiftRepoMock = new Mock<IShiftRepo>();
        var quaries = new ShiftQuaries(shiftRepoMock.Object);

        shiftRepoMock
            .Setup(s => s.GetSchedulesAsync("Jon", "Doe"))
            .ReturnsAsync(employee.Schedules);

        // Act.
        var schedules = await quaries.GetSchedulesAsync("Jon", "Doe");

        // Assert.
        shiftRepoMock.Verify(v => v.GetSchedulesAsync("Jon", "Doe"), Times.Once);
        shiftRepoMock.VerifyNoOtherCalls();

        Assert.Equal(2, schedules.Count());

        Assert.Equal(DateTime.Parse("1 Mar 2023"), schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Morning, schedules.ElementAt(0).Shift);

        Assert.Equal(DateTime.Parse("4 Oct 2024"), schedules.ElementAt(1).Date);
        Assert.Equal(Shift.Day, schedules.ElementAt(1).Shift);
    }

    [Fact]
    public async Task Can_GetSchedulesAsync_ThrowException_WithEmpName()
    {
        // Arrange.
        var shiftRepoMock = new Mock<IShiftRepo>();
        var quaries = new ShiftQuaries(shiftRepoMock.Object);

        shiftRepoMock
            .Setup(s => s.GetSchedulesAsync("Jon", "Doe"))
            .Throws<InvalidOperationException>();

        // Act.
        var testCode = () => quaries.GetSchedulesAsync("Jon", "Doe");

        // Assert.
        await Assert.ThrowsAsync<InvalidOperationException>(testCode);

        shiftRepoMock.Verify(v => v.GetSchedulesAsync("Jon", "Doe"), Times.Once);
        shiftRepoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Can_GetSchedulesAsync_ReturnSchedules_WithDate()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("4 Oct 2024"), Shift.Day);

        var shiftRepoMock = new Mock<IShiftRepo>();
        var quaries = new ShiftQuaries(shiftRepoMock.Object);

        shiftRepoMock
            .Setup(s => s.GetSchedulesAsync(DateTime.Parse("1 Mar 2023")))
            .ReturnsAsync(employee.Schedules.Where(s => s.Date == DateTime.Parse("1 Mar 2023")));

        // Act.
        var schedules = await quaries.GetSchedulesAsync(DateTime.Parse("1 Mar 2023"));

        // Assert.
        shiftRepoMock.Verify(v => v.GetSchedulesAsync(DateTime.Parse("1 Mar 2023")), Times.Once);
        shiftRepoMock.VerifyNoOtherCalls();

        Assert.Single(schedules);

        Assert.Equal(DateTime.Parse("1 Mar 2023"), schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Morning, schedules.ElementAt(0).Shift);
    }

    [Fact]
    public async Task Can_GetSchedulesAsync_ThrowException_WithDate()
    {
        // Arrange.
        var shiftRepoMock = new Mock<IShiftRepo>();
        var quaries = new ShiftQuaries(shiftRepoMock.Object);

        shiftRepoMock
            .Setup(s => s.GetSchedulesAsync(DateTime.Parse("1 Mar 2023")))
            .Throws<InvalidOperationException>();

        // Act.
        var testCode = () => quaries.GetSchedulesAsync(DateTime.Parse("1 Mar 2023"));

        // Assert.
        await Assert.ThrowsAsync<InvalidOperationException>(testCode);

        shiftRepoMock.Verify(v => v.GetSchedulesAsync(DateTime.Parse("1 Mar 2023")), Times.Once);
        shiftRepoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Can_GetSchedulesAsync_ReturnSchedules_WithMonth()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("4 Oct 2024"), Shift.Day);

        var shiftRepoMock = new Mock<IShiftRepo>();
        var quaries = new ShiftQuaries(shiftRepoMock.Object);

        shiftRepoMock
            .Setup(s => s.GetSchedulesAsync(Month.Oct))
            .ReturnsAsync(employee.Schedules.Where(s => s.Date.Month == (int)Month.Oct));

        // Act.
        var schedules = await quaries.GetSchedulesAsync(Month.Oct);

        // Assert.
        shiftRepoMock.Verify(v => v.GetSchedulesAsync(Month.Oct), Times.Once);
        shiftRepoMock.VerifyNoOtherCalls();

        Assert.Single(schedules);

        Assert.Equal(DateTime.Parse("4 Oct 2024"), schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Day, schedules.ElementAt(0).Shift);
    }

    [Fact]
    public async Task Can_GetSchedulesAsync_ThrowException_WithMonth()
    {
        // Arrange.
        var shiftRepoMock = new Mock<IShiftRepo>();
        var quaries = new ShiftQuaries(shiftRepoMock.Object);

        shiftRepoMock
            .Setup(s => s.GetSchedulesAsync(Month.Oct))
            .Throws<InvalidOperationException>();

        // Act.
        var testCode = () => quaries.GetSchedulesAsync(Month.Oct);

        // Assert.
        await Assert.ThrowsAsync<InvalidOperationException>(testCode);

        shiftRepoMock.Verify(v => v.GetSchedulesAsync(Month.Oct), Times.Once);
        shiftRepoMock.VerifyNoOtherCalls();
    }
}
