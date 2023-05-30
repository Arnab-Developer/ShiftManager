namespace ShiftManager.Application.Commands;

public class CreateEmployeeCommand : IRequest<bool>
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}