namespace ShiftManager.Domain;

public class Schedule
{
    private DateTime _date;

    public int Id { get; private set; }

    public DateTime Date
    {
        get => _date;
        private set
        {
            var currentYear = DateTime.Now.Year;
            var nextYear = currentYear + 1;

            if (value.Year < currentYear || value.Year > nextYear)
            {
                throw new ArgumentException($"Invalid {nameof(Date)}");
            }

            _date = value;
        }
    }

    public Shift Shift { get; internal set; }

    public Employee Employee { get; private set; }

    private Schedule()
    {
        _date = DateTime.MinValue;
        Shift = Shift.Day;
        Employee = new Employee("emp1", "emp1");
    }

    public Schedule(DateTime date, Shift shift) : this()
    {
        Date = date;
        Shift = shift;
    }
}