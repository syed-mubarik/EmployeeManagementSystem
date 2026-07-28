using EmployeeManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Application.Mappings;
using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Infrastructure.Repositories;
using EmployeeManagement.Application.Services.Employees;
using EmployeeManagement.Application.Interfaces.Services;
using FluentValidation;
using EmployeeManagement.Application.Validators.Employees;
using EmployeeManagement.API.Middleware;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

// register the validators:
builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeValidator>();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository & Unit of Work
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// AutoMapper
//By scanning the assembly, all profiles are registered automatically.
 
builder.Services.AddAutoMapper(typeof(EmployeeProfile).Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
