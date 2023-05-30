namespace ShiftManager.DomainTest;

public class EmployeeTests
{
    [Fact]
    public void Can_Employee_ReturnProperInitialData()
    {
        // Arrange and Act.
        var employee = new Employee("Jon", "Doe");

        // Assert.
        Assert.Equal("Jon", employee.FirstName);
        Assert.Equal("Doe", employee.LastName);
        Assert.NotNull(employee.Schedules);
        Assert.Empty(employee.Schedules);
    }

    [Fact]
    public void Can_Employee_AddSchedules()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        // Act.
        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("4 Oct 2024"), Shift.Day);

        // Assert.
        Assert.Equal("Jon", employee.FirstName);
        Assert.Equal("Doe", employee.LastName);
        Assert.Equal(2, employee.Schedules.Count);

        Assert.Equal(DateTime.Parse("1 Mar 2023"), employee.Schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Morning, employee.Schedules.ElementAt(0).Shift);

        Assert.Equal(DateTime.Parse("4 Oct 2024"), employee.Schedules.ElementAt(1).Date);
        Assert.Equal(Shift.Day, employee.Schedules.ElementAt(1).Shift);
    }

    [Fact]
    public void Can_Employee_AddSchedules_ThrowException_WithDuplicateSchedules()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");
        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Morning);

        // Act.
        var testCode = () => employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Night);

        // Assert.
        var ex = Assert.Throws<ArgumentException>(testCode);
        Assert.Equal("date already exists", ex.Message);

        Assert.Equal("Jon", employee.FirstName);
        Assert.Equal("Doe", employee.LastName);
        Assert.Equal(1, employee.Schedules.Count);

        Assert.Equal(DateTime.Parse("1 Mar 2023"), employee.Schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Morning, employee.Schedules.ElementAt(0).Shift);
    }

    [Fact]
    public void Can_Employee_UpdateSchedule()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("4 Oct 2024"), Shift.Day);

        var schedule = new Schedule(DateTime.Parse("4 Oct 2024"), Shift.Night);

        // Act.
        employee.UpdateSchedule(schedule);

        // Assert.
        Assert.Equal("Jon", employee.FirstName);
        Assert.Equal("Doe", employee.LastName);
        Assert.Equal(2, employee.Schedules.Count);

        Assert.Equal(DateTime.Parse("1 Mar 2023"), employee.Schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Morning, employee.Schedules.ElementAt(0).Shift);

        Assert.Equal(DateTime.Parse("4 Oct 2024"), employee.Schedules.ElementAt(1).Date);
        Assert.Equal(Shift.Night, employee.Schedules.ElementAt(1).Shift);
    }

    [Fact]
    public void Can_Employee_UpdateSchedule_ThrowException_IfScheduleNotFound()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("4 Oct 2024"), Shift.Day);

        var schedule = new Schedule(DateTime.Parse("2 Oct 2024"), Shift.Night);

        // Act.
        var testCode = () => employee.UpdateSchedule(schedule);

        // Assert.
        Assert.Throws<InvalidOperationException>(testCode);

        Assert.Equal("Jon", employee.FirstName);
        Assert.Equal("Doe", employee.LastName);
        Assert.Equal(2, employee.Schedules.Count);

        Assert.Equal(DateTime.Parse("1 Mar 2023"), employee.Schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Morning, employee.Schedules.ElementAt(0).Shift);

        Assert.Equal(DateTime.Parse("4 Oct 2024"), employee.Schedules.ElementAt(1).Date);
        Assert.Equal(Shift.Day, employee.Schedules.ElementAt(1).Shift);
    }

    [Fact]
    public void Can_Employee_RemoveSchedule()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("4 Oct 2024"), Shift.Day);

        // Act.
        employee.RemoveSchedule(DateTime.Parse("4 Oct 2024"));

        // Assert.
        Assert.Equal("Jon", employee.FirstName);
        Assert.Equal("Doe", employee.LastName);
        Assert.Single(employee.Schedules);

        Assert.Equal(DateTime.Parse("1 Mar 2023"), employee.Schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Morning, employee.Schedules.ElementAt(0).Shift);
    }

    [Fact]
    public void Can_Employee_RemoveSchedule_ThrowException_IfScheduleNotFound()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("4 Oct 2024"), Shift.Day);

        // Act.
        var testCode = () => employee.RemoveSchedule(DateTime.Parse("2 Oct 2024"));

        // Assert.
        Assert.Throws<InvalidOperationException>(testCode);

        Assert.Equal("Jon", employee.FirstName);
        Assert.Equal("Doe", employee.LastName);
        Assert.Equal(2, employee.Schedules.Count);

        Assert.Equal(DateTime.Parse("1 Mar 2023"), employee.Schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Morning, employee.Schedules.ElementAt(0).Shift);

        Assert.Equal(DateTime.Parse("4 Oct 2024"), employee.Schedules.ElementAt(1).Date);
        Assert.Equal(Shift.Day, employee.Schedules.ElementAt(1).Shift);
    }

    [Fact]
    public void Can_Employee_ThrowException_WithNullFirstName()
    {
        // Arrange and Act.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var testCode = () => new Employee(null, "Doe");
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        // Assert.
        var ex = Assert.Throws<ArgumentNullException>(testCode);
        Assert.Equal("Value cannot be null. (Parameter 'FirstName')", ex.Message);
    }

    [Fact]
    public void Can_Employee_ThrowException_WithBlankStringFirstName()
    {
        // Arrange and Act.
        var testCode = () => new Employee("", "Doe");

        // Assert.
        var ex = Assert.Throws<ArgumentNullException>(testCode);
        Assert.Equal("Value cannot be null. (Parameter 'FirstName')", ex.Message);
    }

    [Fact]
    public void Can_Employee_ThrowException_WithEmptyStringFirstName()
    {
        // Arrange and Act.
        var testCode = () => new Employee(string.Empty, "Doe");

        // Assert.
        var ex = Assert.Throws<ArgumentNullException>(testCode);
        Assert.Equal("Value cannot be null. (Parameter 'FirstName')", ex.Message);
    }

    [Fact]
    public void Can_Employee_ThrowException_WithWhiteSpaceFirstName()
    {
        // Arrange and Act.
        var testCode = () => new Employee(" ", "Doe");

        // Assert.
        var ex = Assert.Throws<ArgumentNullException>(testCode);
        Assert.Equal("Value cannot be null. (Parameter 'FirstName')", ex.Message);
    }

    [Fact]
    public void Can_Employee_ThrowException_WithNullLastName()
    {
        // Arrange and Act.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var testCode = () => new Employee("Jon", null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        // Assert.
        var ex = Assert.Throws<ArgumentNullException>(testCode);
        Assert.Equal("Value cannot be null. (Parameter 'LastName')", ex.Message);
    }

    [Fact]
    public void Can_Employee_ThrowException_WithBlankStringLastName()
    {
        // Arrange and Act.
        var testCode = () => new Employee("Jon", "");

        // Assert.
        var ex = Assert.Throws<ArgumentNullException>(testCode);
        Assert.Equal("Value cannot be null. (Parameter 'LastName')", ex.Message);
    }

    [Fact]
    public void Can_Employee_ThrowException_WithEmptyStringLastName()
    {
        // Arrange and Act.
        var testCode = () => new Employee("Jon", string.Empty);

        // Assert.
        var ex = Assert.Throws<ArgumentNullException>(testCode);
        Assert.Equal("Value cannot be null. (Parameter 'LastName')", ex.Message);
    }

    [Fact]
    public void Can_Employee_ThrowException_WithWhiteSpaceLastName()
    {
        // Arrange and Act.
        var testCode = () => new Employee("Jon", " ");

        // Assert.
        var ex = Assert.Throws<ArgumentNullException>(testCode);
        Assert.Equal("Value cannot be null. (Parameter 'LastName')", ex.Message);
    }

    [Fact]
    public void Can_Employee_ThrowException_WithInvalidScheduleMinDate()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("4 Oct 2024"), Shift.Day);

        var previousYear = DateTime.Now.Year - 1;
        var invalidMinDate = DateTime.Parse($"31 Dec {previousYear}");

        // Act.
        var testCode = () => employee.AddSchedule(invalidMinDate, Shift.Day);

        // Assert.
        var ex = Assert.Throws<ArgumentException>(testCode);
        Assert.Equal("Invalid Date", ex.Message);
    }

    [Fact]
    public void Can_Employee_ThrowException_WithInvalidScheduleMaxDate()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Morning);
        employee.AddSchedule(DateTime.Parse("4 Oct 2024"), Shift.Day);

        var yearAfterNext = DateTime.Now.Year + 2;
        var invalidMaxDate = DateTime.Parse($"1 Jan {yearAfterNext}");

        // Act.
        var testCode = () => employee.AddSchedule(invalidMaxDate, Shift.Day);

        // Assert.
        var ex = Assert.Throws<ArgumentException>(testCode);
        Assert.Equal("Invalid Date", ex.Message);
    }

    [Fact]
    public void Can_Employee_ReturnProperData_WithValidScheduleMinDate()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");
        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Morning);

        var inputCurrentYear = DateTime.Now.Year;
        var inputValidMinDate = DateTime.Parse($"1 Jan {inputCurrentYear}");

        var expectedCurrentYear = DateTime.Now.Year;
        var expectedValidMinDate = DateTime.Parse($"1 Jan {expectedCurrentYear}");

        // Act.
        employee.AddSchedule(inputValidMinDate, Shift.Day);

        // Assert.
        Assert.Equal("Jon", employee.FirstName);
        Assert.Equal("Doe", employee.LastName);
        Assert.Equal(2, employee.Schedules.Count);

        Assert.Equal(DateTime.Parse("1 Mar 2023"), employee.Schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Morning, employee.Schedules.ElementAt(0).Shift);

        Assert.Equal(expectedValidMinDate, employee.Schedules.ElementAt(1).Date);
        Assert.Equal(Shift.Day, employee.Schedules.ElementAt(1).Shift);
    }

    [Fact]
    public void Can_Employee_ReturnProperData_WithValidScheduleMaxDate()
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");

        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), Shift.Morning);

        var inputNextYear = DateTime.Now.Year + 1;
        var inputValidMaxDate = DateTime.Parse($"31 Dec {inputNextYear}");

        var expectedNextYear = DateTime.Now.Year + 1;
        var expectedValidMaxDate = DateTime.Parse($"31 Dec {expectedNextYear}");

        // Act.
        employee.AddSchedule(inputValidMaxDate, Shift.Day);

        // Assert.
        Assert.Equal("Jon", employee.FirstName);
        Assert.Equal("Doe", employee.LastName);
        Assert.Equal(2, employee.Schedules.Count);

        Assert.Equal(DateTime.Parse("1 Mar 2023"), employee.Schedules.ElementAt(0).Date);
        Assert.Equal(Shift.Morning, employee.Schedules.ElementAt(0).Shift);

        Assert.Equal(expectedValidMaxDate, employee.Schedules.ElementAt(1).Date);
        Assert.Equal(Shift.Day, employee.Schedules.ElementAt(1).Shift);
    }

    [Theory]
    [InlineData(Shift.Night, Shift.Morning)]
    [InlineData(Shift.Night, Shift.Day)]
    [InlineData(Shift.Night, Shift.Night)]
    public void Can_Employee_ThrowException_WithNextDayContinuousShiftAdd(Shift previousShift, Shift currentShift)
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");
        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), previousShift);

        // Act.
        var testCode = () => employee.AddSchedule(DateTime.Parse("2 Mar 2023"), currentShift);

        // Assert.
        var ex = Assert.Throws<InvalidOperationException>(() => testCode());
        Assert.Equal("Continuous shifts", ex.Message);
    }

    [Theory]
    [InlineData(Shift.Night, Shift.Morning)]
    [InlineData(Shift.Night, Shift.Day)]
    [InlineData(Shift.Night, Shift.Night)]
    public void Can_Employee_DoNotThrowException_WithDifferentDateContinuousShiftAdd(Shift previousShift, Shift currentShift)
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");
        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), previousShift);

        // Act.
        employee.AddSchedule(DateTime.Parse("3 Mar 2023"), currentShift);

        // Assert.
        Assert.Equal("Jon", employee.FirstName);
        Assert.Equal("Doe", employee.LastName);
        Assert.Equal(2, employee.Schedules.Count);

        Assert.Equal(DateTime.Parse("1 Mar 2023"), employee.Schedules.ElementAt(0).Date);
        Assert.Equal(previousShift, employee.Schedules.ElementAt(0).Shift);

        Assert.Equal(DateTime.Parse("3 Mar 2023"), employee.Schedules.ElementAt(1).Date);
        Assert.Equal(currentShift, employee.Schedules.ElementAt(1).Shift);
    }

    [Theory]
    [InlineData(Shift.Night, Shift.Morning)]
    [InlineData(Shift.Night, Shift.Day)]
    [InlineData(Shift.Night, Shift.Night)]
    public void Can_Employee_ThrowException_WithNextDayContinuousShiftUpdate(Shift previousShift, Shift currentShift)
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");
        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), previousShift);
        employee.AddSchedule(DateTime.Parse("2 Mar 2023"), Shift.Off);

        // Act.
        var testCode = () => employee.UpdateSchedule(new Schedule(DateTime.Parse("2 Mar 2023"), currentShift));

        // Assert.
        var ex = Assert.Throws<InvalidOperationException>(() => testCode());
        Assert.Equal("Continuous shifts", ex.Message);
    }

    [Theory]
    [InlineData(Shift.Night, Shift.Morning)]
    [InlineData(Shift.Night, Shift.Day)]
    [InlineData(Shift.Night, Shift.Night)]
    public void Can_Employee_DoNotThrowException_WithDifferentDateContinuousShiftUpdate(Shift previousShift, Shift currentShift)
    {
        // Arrange.
        var employee = new Employee("Jon", "Doe");
        employee.AddSchedule(DateTime.Parse("1 Mar 2023"), previousShift);
        employee.AddSchedule(DateTime.Parse("3 Mar 2023"), Shift.Off);

        // Act.
        employee.UpdateSchedule(new Schedule(DateTime.Parse("3 Mar 2023"), currentShift));

        // Assert.
        Assert.Equal("Jon", employee.FirstName);
        Assert.Equal("Doe", employee.LastName);
        Assert.Equal(2, employee.Schedules.Count);

        Assert.Equal(DateTime.Parse("1 Mar 2023"), employee.Schedules.ElementAt(0).Date);
        Assert.Equal(previousShift, employee.Schedules.ElementAt(0).Shift);

        Assert.Equal(DateTime.Parse("3 Mar 2023"), employee.Schedules.ElementAt(1).Date);
        Assert.Equal(currentShift, employee.Schedules.ElementAt(1).Shift);
    }
}