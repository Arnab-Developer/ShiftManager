namespace ShiftManager.Application.Handlers;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, bool>
{
    private readonly IShiftRepo _shiftRepo;

    public CreateEmployeeCommandHandler(IShiftRepo shiftRepo) => _shiftRepo = shiftRepo;

    public async Task<bool> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = new Employee(request.FirstName, request.LastName);
        await _shiftRepo.CreateEmployeeAsync(employee);
        return true;
    }
}
