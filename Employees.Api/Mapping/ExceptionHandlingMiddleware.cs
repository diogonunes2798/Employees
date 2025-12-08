using Employees.Application.Exceptions;
using Employees.Contracts.Responses;
using FluentValidation;
using System.Net;

namespace Employees.Api.Mapping
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var validationFailureRespose = new ValidationFailureResponse
                {
                    Erros = ex.Errors.Select(x => new ValidationResponse
                    {
                        PropertyName = x.PropertyName,
                        Message = x.ErrorMessage
                    })
                };

                await context.Response.WriteAsJsonAsync(validationFailureRespose);
            }
            catch (ConflictException ex)
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.Response.WriteAsJsonAsync(new
                {
                    title = "Conflict",
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new
                {
                    title = "Internal Server Error",
                    message = ex.Message
                });
            }
        }
    }
}
