using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRental.Infrastructure.Filters
{
    /// <summary>
    /// A global minimal API endpoint filter to execute FluentValidation rules.
    /// Rejects requests returning 400 Bad Request if validation rules fail.
    /// </summary>
    public class ValidationFilter<T> : IEndpointFilter where T : class
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
            
            if (validator != null)
            {
                var targetInstance = context.Arguments.OfType<T>().FirstOrDefault();
                
                if (targetInstance != null)
                {
                    var validationResult = await validator.ValidateAsync(targetInstance);
                    
                    if (!validationResult.IsValid)
                    {
                        var errors = validationResult.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                            
                        return Results.ValidationProblem(errors);
                    }
                }
            }
            
            return await next(context);
        }
    }
}
