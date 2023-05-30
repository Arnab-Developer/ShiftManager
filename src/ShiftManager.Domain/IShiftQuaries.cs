namespace ShiftManager.Domain;

public interface IShiftQuaries
{
    public Task<IEnumerable<Schedule>> GetSchedulesAsync(int employeeId);

    public Task<IEnumerable<Schedule>> GetSchedulesAsync(string firstName, string lastName);

    public Task<IEnumerable<Schedule>> GetSchedulesAsync(DateTime date);

    public Task<IEnumerable<Schedule>> GetSchedulesAsync(Month month);
}
