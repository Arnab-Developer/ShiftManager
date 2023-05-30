using MediatR;
using ShiftManager.Application;
using ShiftManager.Application.Commands;
using ShiftManager.Domain;
using ShiftManager.Infra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateEmployeeCommand>());
builder.Services.AddTransient<IShiftRepo, ShiftRepo>();
builder.Services.AddTransient<IShiftQuaries, ShiftQuaries>();
builder.Services.AddSqlServer<ShiftContext>(builder.Configuration.GetConnectionString("ShiftDb"));

var app = builder.Build();

app.MapGet("get-schedule-by-name", async (IShiftQuaries quaries, string firstName, string lastName) =>
    await quaries.GetSchedulesAsync(firstName, lastName));

app.MapGet("get-schedule-by-date", async (IShiftQuaries quaries, DateTime date) =>
    await quaries.GetSchedulesAsync(date));

app.MapGet("get-schedule-by-month", async (IShiftQuaries quaries, Month month) =>
    await quaries.GetSchedulesAsync(month));

app.MapPost("/create-employee", async (
    IMediator mediator, string firstName, string lastName) =>
{
    var command = new CreateEmployeeCommand
    {
        FirstName = firstName,
        LastName = lastName
    };
    await mediator.Send(command);
});

app.MapPost("/update-employee", async (
    IMediator mediator, int employeeId, string firstName, string lastName) =>
{
    var command = new UpdateEmployeeCommand
    {
        EmployeeId = employeeId,
        FirstName = firstName,
        LastName = lastName
    };
    await mediator.Send(command);
});

app.MapPut("/add-schedule", async (
    IMediator mediator, int employeeId, DateTime date, Shift shift) =>
{
    var command = new AddScheduleCommand
    {
        EmployeeId = employeeId,
        Date = date,
        Shift = shift
    };
    await mediator.Send(command);
});

app.MapPut("/update-schedule", async (
    IMediator mediator, int employeeId, DateTime date, Shift shift) =>
{
    var command = new UpdateScheduleCommand
    {
        EmployeeId = employeeId,
        Date = date,
        Shift = shift
    };
    await mediator.Send(command);
});

app.MapDelete("/delete-schedule", async (
    IMediator mediator, int employeeId, DateTime date) =>
{
    var command = new DeleteScheduleCommand
    {
        EmployeeId = employeeId,
        Date = date
    };
    await mediator.Send(command);
});

app.MapDelete("/delete-employee", async (
    IMediator mediator, int employeeId) =>
{
    var command = new DeleteEmployeeCommand
    {
        EmployeeId = employeeId
    };
    await mediator.Send(command);
});

app.Run();