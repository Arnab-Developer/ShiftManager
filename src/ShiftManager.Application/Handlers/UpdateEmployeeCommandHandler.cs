namespace ShiftManager.Application.Handlers;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, bool>
{
    private readonly IShiftRepo _shiftRepo;

    public UpdateEmployeeCommandHandler(IShiftRepo shiftRepo) => _shiftRepo = shiftRepo;

    public async Task<bool> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _shiftRepo.GetEmployeeAsync(request.EmployeeId);

        employee.FirstName = request.FirstName;
        employee.LastName = request.LastName;

        await _shiftRepo.UpdateEmployeeAsync(employee);

        return true;
    }
}
