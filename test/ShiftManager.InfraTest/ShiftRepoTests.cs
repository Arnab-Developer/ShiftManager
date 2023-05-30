using Microsoft.EntityFrameworkCore;
using ShiftManager.Domain;
using ShiftManager.Infra;

namespace ShiftManager.InfraTest;

public class ShiftRepoTests
{
    [Fact]
    public async Task Can_CreateEmployeeAsync_WorkProperly()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Feb 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("2 Feb 2023"), Shift.Day);

        var option = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using (var context = new ShiftContext(option))
        {
            var repo = new ShiftRepo(context);

            // Act.
            await repo.CreateEmployeeAsync(employee);
        }

        // Assert.
        var assertOption = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using var assertContext = new ShiftContext(assertOption);

        var dbEmployee = await assertContext.Employees
            .Include(e => e.Schedules)
            .FirstAsync(e => e.FirstName == "Jon" && e.LastName == "Doe");

        Assert.Equal(DateTime.Parse("1 Feb 2023"), dbEmployee.Schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Morning, dbEmployee.Schedules.ElementAt(0).Shift);

        Assert.Equal(DateTime.Parse("2 Feb 2023"), dbEmployee.Schedules.ElementAt(1).Date);
        Assert.Equal(Shift.Day, dbEmployee.Schedules.ElementAt(1).Shift);

        // Clean up
        DropDb();
    }

    [Fact]
    public async Task Can_UpdateEmployeeAsync_WorkProperly()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Feb 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("2 Feb 2023"), Shift.Day);

        var arrangeOption = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using (var arrangeContext = new ShiftContext(arrangeOption))
        {
            await arrangeContext.Employees.AddAsync(employee);
            await arrangeContext.SaveChangesAsync();
        }

        var option = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using (var context = new ShiftContext(option))
        {
            var repo = new ShiftRepo(context);

            // Act.
            var dbEmployee = await repo.GetEmployeeAsync(1);

            dbEmployee.FirstName = "Jon Updated";
            dbEmployee.UpdateSchedule(new Schedule(DateTime.Parse("2 Feb 2023"), Shift.Off));
            dbEmployee.AddSchedule(DateTime.Parse("10 Oct 2023"), Shift.Night);
            dbEmployee.RemoveSchedule(DateTime.Parse("1 Feb 2023"));

            await repo.UpdateEmployeeAsync(dbEmployee);
        }

        // Assert.
        var assertOption = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using var assertContext = new ShiftContext(assertOption);

        var assertEmployee = await assertContext.Employees
            .Include(e => e.Schedules)
            .FirstAsync(e => e.Id == 1);

        Assert.Equal("Jon Updated", assertEmployee.FirstName);
        Assert.Equal("Doe", assertEmployee.LastName);

        Assert.Equal(2, assertEmployee.Schedules.Count);

        Assert.Equal(DateTime.Parse("2 Feb 2023"), assertEmployee.Schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Off, assertEmployee.Schedules.ElementAt(0).Shift);

        Assert.Equal(DateTime.Parse("10 Oct 2023"), assertEmployee.Schedules.ElementAt(1).Date);
        Assert.Equal(Shift.Night, assertEmployee.Schedules.ElementAt(1).Shift);

        // Clean up
        DropDb();
    }

    [Fact]
    public async Task Can_DeleteEmployeeAsync_WorkProperly()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Feb 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("2 Feb 2023"), Shift.Day);

        var arrangeOption = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using (var arrangeContext = new ShiftContext(arrangeOption))
        {
            await arrangeContext.Employees.AddAsync(employee);
            await arrangeContext.SaveChangesAsync();
        }

        var option = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using (var context = new ShiftContext(option))
        {
            var repo = new ShiftRepo(context);

            // Act.
            await repo.DeleteEmployeeAsync(1);
        }

        // Assert.
        var assertOption = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using var assertContext = new ShiftContext(assertOption);

        var dbEmployee = await assertContext.Employees
            .Include(e => e.Schedules)
            .FirstOrDefaultAsync(e => e.FirstName == "Jon" && e.LastName == "Doe");

        Assert.Null(dbEmployee);

        // Clean up
        DropDb();
    }

    [Fact]
    public async Task Can_GetSchedulesAsync_ReturnSchedules_WithEmpId()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Feb 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("2 Feb 2023"), Shift.Day);

        var arrangeOption = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using (var arrangeContext = new ShiftContext(arrangeOption))
        {
            await arrangeContext.Employees.AddAsync(employee);
            await arrangeContext.SaveChangesAsync();
        }

        var option = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using var context = new ShiftContext(option);
        var repo = new ShiftRepo(context);

        // Act.
        var schedules = await repo.GetSchedulesAsync(1);

        // Assert.
        Assert.Equal(2, schedules.Count());

        Assert.Equal(DateTime.Parse("1 Feb 2023"), schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Morning, schedules.ElementAt(0).Shift);

        Assert.Equal(DateTime.Parse("2 Feb 2023"), schedules.ElementAt(1).Date);
        Assert.Equal(Shift.Day, schedules.ElementAt(1).Shift);

        Assert.Equal("Jon", schedules.ElementAt(0).Employee.FirstName);
        Assert.Equal("Doe", schedules.ElementAt(0).Employee.LastName);

        // Clean up
        DropDb();
    }

    [Fact]
    public async Task Can_GetSchedulesAsync_ReturnSchedules_WithEmpName()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Feb 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("2 Feb 2023"), Shift.Day);

        var arrangeOption = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using (var arrangeContext = new ShiftContext(arrangeOption))
        {
            await arrangeContext.Employees.AddAsync(employee);
            await arrangeContext.SaveChangesAsync();
        }

        var option = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using var context = new ShiftContext(option);
        var repo = new ShiftRepo(context);

        // Act.
        var schedules = await repo.GetSchedulesAsync("Jon", "Doe");

        // Assert.
        Assert.Equal(2, schedules.Count());

        Assert.Equal(DateTime.Parse("1 Feb 2023"), schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Morning, schedules.ElementAt(0).Shift);

        Assert.Equal(DateTime.Parse("2 Feb 2023"), schedules.ElementAt(1).Date);
        Assert.Equal(Shift.Day, schedules.ElementAt(1).Shift);

        Assert.Equal("Jon", schedules.ElementAt(0).Employee.FirstName);
        Assert.Equal("Doe", schedules.ElementAt(0).Employee.LastName);

        // Clean up
        DropDb();
    }

    [Fact]
    public async Task Can_GetSchedulesAsync_ReturnSchedules_WithDate()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Feb 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("2 Feb 2023"), Shift.Day);

        var arrangeOption = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using (var arrangeContext = new ShiftContext(arrangeOption))
        {
            await arrangeContext.Employees.AddAsync(employee);
            await arrangeContext.SaveChangesAsync();
        }

        var option = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using var context = new ShiftContext(option);
        var repo = new ShiftRepo(context);

        // Act.
        var schedules = await repo.GetSchedulesAsync(DateTime.Parse("2 Feb 2023"));

        // Assert.
        Assert.Single(schedules);

        Assert.Equal(DateTime.Parse("2 Feb 2023"), schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Day, schedules.ElementAt(0).Shift);

        Assert.Equal("Jon", schedules.ElementAt(0).Employee.FirstName);
        Assert.Equal("Doe", schedules.ElementAt(0).Employee.LastName);

        // Clean up
        DropDb();
    }

    [Fact]
    public async Task Can_GetSchedulesAsync_ReturnSchedules_WithMonth()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Feb 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("2 Feb 2024"), Shift.Day);

        var arrangeOption = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using (var arrangeContext = new ShiftContext(arrangeOption))
        {
            await arrangeContext.Employees.AddAsync(employee);
            await arrangeContext.SaveChangesAsync();
        }

        var option = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using var context = new ShiftContext(option);
        var repo = new ShiftRepo(context);

        // Act.
        var schedules = await repo.GetSchedulesAsync(Month.Feb);

        // Assert.
        Assert.Single(schedules);

        Assert.Equal(DateTime.Parse("1 Feb 2023"), schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Morning, schedules.ElementAt(0).Shift);

        Assert.Equal("Jon", schedules.ElementAt(0).Employee.FirstName);
        Assert.Equal("Doe", schedules.ElementAt(0).Employee.LastName);

        // Clean up
        DropDb();
    }

    [Fact]
    public async Task Can_GetSchedulesAsync_ReturnEmptySchedules_IfNotFound()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Feb 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("2 Feb 2023"), Shift.Day);

        var arrangeOption = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using (var arrangeContext = new ShiftContext(arrangeOption))
        {
            await arrangeContext.Employees.AddAsync(employee);
            await arrangeContext.SaveChangesAsync();
        }

        var option = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using var context = new ShiftContext(option);
        var repo = new ShiftRepo(context);

        // Act.
        var schedules = await repo.GetSchedulesAsync("Bob", "Doe");

        // Assert.
        Assert.NotNull(schedules);
        Assert.Empty(schedules);

        // Clean up
        DropDb();
    }

    private static void DropDb()
    {
        var options = new DbContextOptionsBuilder<ShiftContext>()
            .UseInMemoryDatabase("ShiftDb")
            .Options;

        using var context = new ShiftContext(options);
        context.Database.EnsureDeleted();
    }
}