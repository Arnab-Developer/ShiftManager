namespace ShiftManager.Application.Commands;

public class DeleteEmployeeCommand : IRequest<bool>
{
    public required int EmployeeId { get; set; }
}
