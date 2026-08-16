using EmployeeManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EmployeeManagement.Application.Configuration;
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
using EmployeeManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using EmployeeManagement.Application.Services.Authentication;
using Microsoft.OpenApi.Models;



var builder = WebApplication.CreateBuilder(args);

// Register JwtSettings
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

// Add services to the container.

builder.Services.AddControllers();

// register the validators:  FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeValidator>();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

//  AddIdentity configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Jwt Authentication

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!)),

            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateLifetime = true,

            ClockSkew = TimeSpan.Zero
        };
    });

// Unit of Work & Repository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDesignationRepository, DesignationRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

// Services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDesignationService, DesignationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

// AutoMapper
//By scanning the assembly, all profiles are registered automatically.

builder.Services.AddAutoMapper(typeof(EmployeeProfile).Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Enable Authoriz button on swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
