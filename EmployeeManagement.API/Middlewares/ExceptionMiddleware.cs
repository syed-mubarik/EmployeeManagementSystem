using EmployeeManagement.Application.Exceptions;
using EmployeeManagement.API.Models;
using FluentValidation;
using System.Text.Json;

namespace EmployeeManagement.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                var response = new ErrorResponse();

                switch (ex)
                {
                    case ValidationException validationException:

                        context.Response.StatusCode = StatusCodes.Status400BadRequest;

                        response.Message = "Validation Failed";

                        response.Errors = validationException.Errors
                            .Select(e => e.ErrorMessage)
                            .ToList();

                        break;

                    case KeyNotFoundException:

                        context.Response.StatusCode = StatusCodes.Status404NotFound;

                        response.Message = ex.Message;

                        break;

                    case DuplicateRecordException:

                        context.Response.StatusCode = StatusCodes.Status409Conflict;

                        response.Message = ex.Message;

                        break;

                    default:

                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                       // for Debugging
                       // response.Message = ex.Message;

                        response.Message = "An unexpected error occurred.";

                        break;
                }

                var json = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);
            }
        }
    }
}