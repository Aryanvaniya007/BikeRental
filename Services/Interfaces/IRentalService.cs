using BikeRental.DTOs.Requests;
using BikeRental.DTOs.Responses;

namespace BikeRental.Services.Interfaces
{
    public interface IRentalService
    {
        Task<(bool IsSuccess, string Message, RideStartedDto? Data)> StartRideAsync(int userId, StartRideDto request);
        Task<(bool IsSuccess, string Message, RideEndedDto? Data)> EndRideAsync(int userId, EndRideDto request);
        Task<IEnumerable<ActiveRentalDto>> GetActiveRentalsAsync();
        Task<IEnumerable<RentalHistoryDto>> GetUserRentalsAsync(int userId);
    }
}
