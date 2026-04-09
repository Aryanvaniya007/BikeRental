using BikeRental.DTOs.Requests;
using BikeRental.DTOs.Responses;

namespace BikeRental.Services.Interfaces
{
    public interface IWalletService
    {
        Task<WalletInfoDto> GetWalletInfoAsync(int userId);
        Task<(bool IsSuccess, string Message, WalletInfoDto? Data)> TopUpAsync(int userId, TopUpRequestDto request);
    }
}
