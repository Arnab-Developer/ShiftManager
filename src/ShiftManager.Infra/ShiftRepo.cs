namespace ShiftManager.Infra;

public class ShiftRepo : IShiftRepo
{
    private readonly ShiftContext _context;

    public ShiftRepo(ShiftContext context) => _context = context;

    public async Task CreateEmployeeAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateEmployeeAsync(Employee employee) => await _context.SaveChangesAsync();

    public async Task DeleteEmployeeAsync(int employeeId)
    {
        var employee = await _context.Employees.FirstAsync(e => e.Id == employeeId);
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }

    public async Task<Employee> GetEmployeeAsync(int employeeId) =>
        await _context.Employees
            .Include(e => e.Schedules)
            .FirstAsync(e => e.Id == employeeId);

    public async Task<IEnumerable<Schedule>> GetSchedulesAsync(int employeeId) =>
        await _context.Schedules
            .Include(s => s.Employee)
            .AsNoTracking()
            .Where(s => s.Employee.Id == employeeId)
            .ToListAsync();

    public async Task<IEnumerable<Schedule>> GetSchedulesAsync(string firstName, string lastName) =>
        await _context.Schedules
            .Include(s => s.Employee)
            .AsNoTracking()
            .Where(s => s.Employee.FirstName == firstName && s.Employee.LastName == lastName)
            .ToListAsync();

    public async Task<IEnumerable<Schedule>> GetSchedulesAsync(DateTime date) =>
        await _context.Schedules
            .Include(s => s.Employee)
            .AsNoTracking()
            .Where(s => s.Date == date)
            .ToListAsync();

    public async Task<IEnumerable<Schedule>> GetSchedulesAsync(Month month) =>
        await _context.Schedules
            .Include(s => s.Employee)
            .AsNoTracking()
            .Where(s => s.Date.Month == (int)month && s.Date.Year == DateTime.Now.Year)
            .ToListAsync();
}