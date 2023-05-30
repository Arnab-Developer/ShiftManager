namespace ShiftManager.Domain;

public interface IShiftRepo
{
    public Task CreateEmployeeAsync(Employee employee);

    public Task UpdateEmployeeAsync(Employee employee);

    public Task DeleteEmployeeAsync(int employeeId);

    public Task<Employee> GetEmployeeAsync(int employeeId);

    public Task<IEnumerable<Schedule>> GetSchedulesAsync(int employeeId);

    public Task<IEnumerable<Schedule>> GetSchedulesAsync(string firstName, string lastName);

    public Task<IEnumerable<Schedule>> GetSchedulesAsync(DateTime date);

    public Task<IEnumerable<Schedule>> GetSchedulesAsync(Month month);
}