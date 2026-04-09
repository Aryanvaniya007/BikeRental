using BikeRental.DTOs.Requests;
using BikeRental.Infrastructure.Filters;
using BikeRental.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BikeRental.Endpoints
{
    public static class BikeEndpoints
    {
        public static RouteGroupBuilder MapBikeEndpoints(this RouteGroupBuilder group)
        {
            var bikeGroup = group.MapGroup("/bikes");

            // Publicly accessible to view bikes
            bikeGroup.MapGet("/", async (IBikeService bikeService) =>
            {
                var bikes = await bikeService.GetBikesAsync();
                return Results.Ok(bikes);
            });

            // RBAC: Requires Auth to report maintenance
            bikeGroup.MapPost("/{id}/maintenance", async (int id, ReportMaintenanceDto request, IBikeService bikeService) =>
            {
                var result = await bikeService.ReportMaintenanceAsync(id, request);
                return result.IsSuccess 
                    ? Results.Ok(result.Data)
                    : Results.NotFound(result.Message);
            })
            .AddEndpointFilter<ValidationFilter<ReportMaintenanceDto>>()
            .RequireAuthorization();

            // RBAC: Requires Admin strictly for CRUD operations
            var adminEndpoints = bikeGroup.MapGroup("/")
                .RequireAuthorization(policy => policy.RequireRole("Admin"));

            adminEndpoints.MapPost("", async (CreateBikeDto request, IBikeService bikeService) =>
            {
                var result = await bikeService.CreateBikeAsync(request);
                return result.IsSuccess
                    ? Results.Created($"/bikes/{result.Data?.BikeId}", result.Data)
                    : Results.BadRequest(result.Message);
            })
            .AddEndpointFilter<ValidationFilter<CreateBikeDto>>();

            adminEndpoints.MapPut("{id}", async (int id, UpdateBikeDto request, IBikeService bikeService) =>
            {
                var result = await bikeService.UpdateBikeAsync(id, request);
                return result.IsSuccess
                    ? Results.Ok(result.Data)
                    : Results.NotFound(result.Message);
            })
            .AddEndpointFilter<ValidationFilter<UpdateBikeDto>>();

            adminEndpoints.MapDelete("{id}", async (int id, IBikeService bikeService) =>
            {
                var result = await bikeService.DeleteBikeAsync(id);
                return result.IsSuccess
                    ? Results.Ok(new { message = result.Message })
                    : Results.BadRequest(new { message = result.Message });
            });

            return group;
        }
    }
}
