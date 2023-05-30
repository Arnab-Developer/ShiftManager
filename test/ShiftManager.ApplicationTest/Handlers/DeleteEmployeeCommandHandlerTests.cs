namespace ShiftManager.ApplicationTest.Handlers;

public class DeleteEmployeeCommandHandlerTests
{
    [Fact]
    public async Task Can_DeleteEmployeeCommandHandler_WorksProperly()
    {
        // Arrange.
        var command = new DeleteEmployeeCommand { EmployeeId = 1 };
        var shiftRepoMock = new Mock<IShiftRepo>();
        var handler = new DeleteEmployeeCommandHandler(shiftRepoMock.Object);

        shiftRepoMock
            .Setup(s => s.DeleteEmployeeAsync(1))
            .Returns(Task.FromResult(true));

        // Act.
        var isSuccess = await handler.Handle(command, CancellationToken.None);

        // Assert.
        Assert.True(isSuccess);
        shiftRepoMock.Verify(v => v.DeleteEmployeeAsync(1), Times.Once);
        shiftRepoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Can_DeleteEmployeeCommandHandler_ThrowException()
    {
        // Arrange.
        var command = new DeleteEmployeeCommand { EmployeeId = 1 };
        var shiftRepoMock = new Mock<IShiftRepo>();
        var handler = new DeleteEmployeeCommandHandler(shiftRepoMock.Object);

        shiftRepoMock
            .Setup(s => s.DeleteEmployeeAsync(1))
            .Throws<InvalidOperationException>();

        // Act.
        var testCode = () => handler.Handle(command, CancellationToken.None);

        // Assert.
        await Assert.ThrowsAsync<InvalidOperationException>(testCode);
        shiftRepoMock.Verify(v => v.DeleteEmployeeAsync(1), Times.Once);
    }
}
