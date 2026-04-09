using BikeRental.DTOs.Requests;
using BikeRental.DTOs.Responses;

namespace BikeRental.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool IsSuccess, string Message, string? DemoOtp)> RequestOtpAsync(RequestOtpDto request);
        Task<(bool IsSuccess, string Message, AuthResponseDto? Data)> VerifyOtpAsync(VerifyOtpDto request);
    }
}
