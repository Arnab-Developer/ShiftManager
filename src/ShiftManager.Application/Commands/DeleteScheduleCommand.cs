namespace ShiftManager.Application.Commands;

public class DeleteScheduleCommand : IRequest<bool>
{
    public required int EmployeeId { get; set; }

    public required DateTime Date { get; set; }
}
