using BikeRental.DTOs.Requests;
using BikeRental.Infrastructure.Filters;
using BikeRental.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace BikeRental.Endpoints
{
    public static class WalletEndpoints
    {
        public static RouteGroupBuilder MapWalletEndpoints(this RouteGroupBuilder group)
        {
            var walletGroup = group.MapGroup("/wallet").RequireAuthorization();

            walletGroup.MapGet("/", async (IWalletService walletService, HttpContext httpContext) =>
            {
                var userIdStr = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdStr, out int userId)) return Results.Unauthorized();

                var info = await walletService.GetWalletInfoAsync(userId);
                return Results.Ok(info);
            });

            walletGroup.MapPost("/topup", async (TopUpRequestDto request, IWalletService walletService, HttpContext httpContext) =>
            {
                var userIdStr = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdStr, out int userId)) return Results.Unauthorized();

                var result = await walletService.TopUpAsync(userId, request);
                return result.IsSuccess 
                    ? Results.Ok(result.Data)
                    : Results.BadRequest(new { message = result.Message });
            })
            .AddEndpointFilter<ValidationFilter<TopUpRequestDto>>();

            return group;
        }
    }
}
