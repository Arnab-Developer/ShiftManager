namespace ShiftManager.Application.Commands;

public class UpdateScheduleCommand : IRequest<bool>
{
    public required int EmployeeId { get; set; }

    public required DateTime Date { get; set; }

    public required Shift Shift { get; set; }
}
