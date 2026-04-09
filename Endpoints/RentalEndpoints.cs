using BikeRental.DTOs.Requests;
using BikeRental.Infrastructure.Filters;
using BikeRental.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace BikeRental.Endpoints
{
    public static class RentalEndpoints
    {
        public static RouteGroupBuilder MapRentalEndpoints(this RouteGroupBuilder group)
        {
            var rentalGroup = group.MapGroup("/rentals").RequireAuthorization();

            rentalGroup.MapPost("/start", async (StartRideDto request, IRentalService rentalService, HttpContext httpContext) =>
            {
                var userIdStr = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdStr, out int userId)) return Results.Unauthorized();

                var result = await rentalService.StartRideAsync(userId, request);
                return result.IsSuccess 
                    ? Results.Ok(result.Data)
                    : Results.BadRequest(result.Message);
            })
            .AddEndpointFilter<ValidationFilter<StartRideDto>>();

            rentalGroup.MapPost("/end", async (EndRideDto request, IRentalService rentalService, HttpContext httpContext) =>
            {
                var userIdStr = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdStr, out int userId)) return Results.Unauthorized();

                var result = await rentalService.EndRideAsync(userId, request);
                return result.IsSuccess 
                    ? Results.Ok(result.Data)
                    : Results.BadRequest(result.Message);
            })
            .AddEndpointFilter<ValidationFilter<EndRideDto>>();
            
            // User: View personal rental history
            rentalGroup.MapGet("/my", async (IRentalService rentalService, HttpContext httpContext) =>
            {
                var userIdStr = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdStr, out int userId)) return Results.Unauthorized();

                var rentals = await rentalService.GetUserRentalsAsync(userId);
                return Results.Ok(rentals);
            });

            // RBAC: Requires Admin strictly to view all rentals
            rentalGroup.MapGet("/active", async (IRentalService rentalService) =>
            {
                var activeRentals = await rentalService.GetActiveRentalsAsync();
                return Results.Ok(activeRentals);
            })
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

            return group;
        }
    }
}
