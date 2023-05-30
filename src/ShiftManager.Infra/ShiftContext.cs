namespace ShiftManager.Infra;

public class ShiftContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }

    public DbSet<Schedule> Schedules { get; set; }

    public ShiftContext(DbContextOptions<ShiftContext> options) : base(options)
    {
        Employees = Set<Employee>();
        Schedules = Set<Schedule>();
    }
}