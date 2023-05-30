namespace ShiftManager.Application.Commands;

public class UpdateEmployeeCommand : IRequest<bool>
{
    public required int EmployeeId { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}
