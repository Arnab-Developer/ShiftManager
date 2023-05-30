namespace ShiftManager.Application.Handlers;

public class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleCommand, bool>
{
    private readonly IShiftRepo _shiftRepo;

    public DeleteScheduleCommandHandler(IShiftRepo shiftRepo) => _shiftRepo = shiftRepo;

    public async Task<bool> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
    {
        var employee = await _shiftRepo.GetEmployeeAsync(request.EmployeeId);
        employee.RemoveSchedule(request.Date);
        await _shiftRepo.UpdateEmployeeAsync(employee);

        return true;
    }
}
