namespace ShiftManager.Application.Handlers;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
{
    private readonly IShiftRepo _shiftRepo;

    public DeleteEmployeeCommandHandler(IShiftRepo shiftRepo) => _shiftRepo = shiftRepo;

    public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        await _shiftRepo.DeleteEmployeeAsync(request.EmployeeId);
        return true;
    }
}
