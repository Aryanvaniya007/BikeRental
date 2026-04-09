using BikeRental.DTOs.Requests;
using BikeRental.Infrastructure.Filters;
using BikeRental.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BikeRental.Endpoints
{
    public static class AuthEndpoints
    {
        public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder group)
        {
            var authGroup = group.MapGroup("/auth");

            authGroup.MapPost("/request-otp", async (RequestOtpDto request, IAuthService authService) =>
            {
                var result = await authService.RequestOtpAsync(request);
                return result.IsSuccess 
                    ? Results.Ok(new { message = result.Message, demoOtp = result.DemoOtp })
                    : Results.BadRequest(new { message = result.Message });
            })
            .AddEndpointFilter<ValidationFilter<RequestOtpDto>>();

            authGroup.MapPost("/verify-otp", async (VerifyOtpDto request, IAuthService authService) =>
            {
                var result = await authService.VerifyOtpAsync(request);
                return result.IsSuccess 
                    ? Results.Ok(result.Data)
                    : Results.Unauthorized();
            })
            .AddEndpointFilter<ValidationFilter<VerifyOtpDto>>();

            return group;
        }
    }
}
