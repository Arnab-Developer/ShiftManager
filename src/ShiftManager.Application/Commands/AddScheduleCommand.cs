namespace ShiftManager.Application.Commands;

public class AddScheduleCommand : IRequest<bool>
{
    public required int EmployeeId { get; set; }

    public required DateTime Date { get; set; }

    public required Shift Shift { get; set; }
}
