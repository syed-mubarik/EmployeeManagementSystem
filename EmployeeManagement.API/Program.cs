using EmployeeManagement.API.Middleware;
using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Application.Interfaces.Services;
using EmployeeManagement.Application.Mappings;
using EmployeeManagement.Application.Services.Departments;
using EmployeeManagement.Application.Services.Designations;
using EmployeeManagement.Application.Services.Employees;
using EmployeeManagement.Application.Validators.Employees;
using EmployeeManagement.Infrastructure.Repositories;
using EmployeeManagement.Persistence.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

// register the validators:  FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeValidator>();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Unit of Work & Repository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDesignationRepository, DesignationRepository>();

// Services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDesignationService, DesignationService>();

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
