using BikeRental.DTOs.Requests;
using BikeRental.DTOs.Responses;

namespace BikeRental.Services.Interfaces
{
    public interface IBikeService
    {
        Task<IEnumerable<BikeInfoDto>> GetBikesAsync();
        Task<(bool IsSuccess, string Message, MaintenanceReportDto? Data)> ReportMaintenanceAsync(int bikeId, ReportMaintenanceDto request);
        
        // CRUD Operations
        Task<(bool IsSuccess, string Message, BikeInfoDto? Data)> CreateBikeAsync(CreateBikeDto request);
        Task<(bool IsSuccess, string Message, BikeInfoDto? Data)> UpdateBikeAsync(int bikeId, UpdateBikeDto request);
        Task<(bool IsSuccess, string Message)> DeleteBikeAsync(int bikeId);
    }
}
