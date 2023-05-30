namespace ShiftManager.Application.Handlers;

public class AddScheduleCommandCommandHandler : IRequestHandler<AddScheduleCommand, bool>
{
    private readonly IShiftRepo _shiftRepo;

    public AddScheduleCommandCommandHandler(IShiftRepo shiftRepo) => _shiftRepo = shiftRepo;

    public async Task<bool> Handle(AddScheduleCommand request, CancellationToken cancellationToken)
    {
        var employee = await _shiftRepo.GetEmployeeAsync(request.EmployeeId);
        employee.AddSchedule(request.Date, request.Shift);
        await _shiftRepo.UpdateEmployeeAsync(employee);

        return true;
    }
}
