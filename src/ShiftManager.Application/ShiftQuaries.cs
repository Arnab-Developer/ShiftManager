namespace ShiftManager.Application;

public class ShiftQuaries : IShiftQuaries
{
    private readonly IShiftRepo _shiftRepo;

    public ShiftQuaries(IShiftRepo shiftRepo) => _shiftRepo = shiftRepo;

    public async Task<IEnumerable<Schedule>> GetSchedulesAsync(int employeeId)
    {
        var schedules = await _shiftRepo.GetSchedulesAsync(employeeId);
        RemoveCyclickRef(schedules);
        return schedules;
    }

    public async Task<IEnumerable<Schedule>> GetSchedulesAsync(string firstName, string lastName)
    {
        var schedules = await _shiftRepo.GetSchedulesAsync(firstName, lastName);
        RemoveCyclickRef(schedules);
        return schedules;
    }

    public async Task<IEnumerable<Schedule>> GetSchedulesAsync(DateTime date)
    {
        var schedules = await _shiftRepo.GetSchedulesAsync(date);
        RemoveCyclickRef(schedules);
        return schedules;
    }

    public async Task<IEnumerable<Schedule>> GetSchedulesAsync(Month month)
    {
        var schedules = await _shiftRepo.GetSchedulesAsync(month);
        RemoveCyclickRef(schedules);
        return schedules;
    }

    private static void RemoveCyclickRef(IEnumerable<Schedule> schedules)
    {
        foreach (var schedule in schedules)
        {
            if (schedule.Employee.Schedules.Any())
            {
                schedule.Employee.RemoveSchedule(schedule.Date);
            }
        }
    }
}