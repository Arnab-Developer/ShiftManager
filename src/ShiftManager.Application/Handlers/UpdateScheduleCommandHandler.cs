namespace ShiftManager.Application.Handlers;

public class UpdateScheduleCommandHandler : IRequestHandler<UpdateScheduleCommand, bool>
{
    private readonly IShiftRepo _shiftRepo;

    public UpdateScheduleCommandHandler(IShiftRepo shiftRepo) => _shiftRepo = shiftRepo;

    public async Task<bool> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
    {
        var employee = await _shiftRepo.GetEmployeeAsync(request.EmployeeId);
        employee.UpdateSchedule(new Schedule(request.Date, request.Shift));
        await _shiftRepo.UpdateEmployeeAsync(employee);

        return true;
    }
}
