using GymBookingSystem.Application.Interfaces;
using GymBookingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using GymBookingSystem.Application.Services;
using GymBookingSystem.API;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");



builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

builder.Services.AddScoped<IGymClassService, GymClassService>();

builder.Services.AddScoped<IGymClassService, GymClassService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();

builder.Services.AddScoped<IClassScheduleService, ClassScheduleService>();

builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();


app.UseExceptionHandler();

// ... the rest as before (MapOpenApi, UseHttpsRedirection, ...)


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
