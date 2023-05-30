namespace ShiftManager.Domain;

public class Employee
{
    private string _firstName;
    private string _lastName;
    private readonly IList<Schedule> _schedules;

    public int Id { get; private set; }

    public string FirstName
    {
        get => _firstName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(FirstName));
            }

            _firstName = value;
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(LastName));
            }

            _lastName = value;
        }
    }

    public IReadOnlyCollection<Schedule> Schedules => _schedules.AsReadOnly();

    private Employee()
    {
        _firstName = string.Empty;
        _lastName = string.Empty;
        _schedules = new List<Schedule>();
    }

    public Employee(string firstName, string lastName) : this()
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public void AddSchedule(DateTime date, Shift shift)
    {
        if (Schedules.Any(s => s.Date == date))
        {
            throw new ArgumentException($"{nameof(date)} already exists");
        }
        if (IsContinuousShifts(date, shift))
        {
            throw new InvalidOperationException("Continuous shifts");
        }

        var schedule = new Schedule(date, shift);
        _schedules.Add(schedule);
    }

    public void UpdateSchedule(Schedule schedule)
    {
        if (IsContinuousShifts(schedule.Date, schedule.Shift))
        {
            throw new InvalidOperationException("Continuous shifts");
        }

        var sch = _schedules.Single(s => s.Date == schedule.Date);
        sch.Shift = schedule.Shift;
    }

    public void RemoveSchedule(DateTime date)
    {
        var schedule = _schedules.Single(s => s.Date == date);
        _schedules.Remove(schedule);
    }

    private bool IsContinuousShifts(DateTime date, Shift shift)
    {
        var previousDate = date.AddDays(-1);
        var previousDateSchedule = _schedules.FirstOrDefault(s => s.Date == previousDate);

        return previousDateSchedule is not null &&
            previousDateSchedule.Shift == Shift.Night &&
            shift != Shift.Off;
    }
}