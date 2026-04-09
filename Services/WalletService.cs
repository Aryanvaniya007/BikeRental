using BikeRental.DTOs.Requests;
using BikeRental.DTOs.Responses;
using BikeRental.Infrastructure.Data;
using BikeRental.Models;
using BikeRental.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Services
{
    public class WalletService : IWalletService
    {
        private readonly ApplicationDbContext _context;

        public WalletService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WalletInfoDto> GetWalletInfoAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            var transactions = await _context.WalletTransactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .Take(10)
                .Select(t => new WalletTransactionDto
                {
                    TransactionId = t.WalletTransactionId,
                    Amount = t.Amount,
                    Type = t.Type.ToString(),
                    Description = t.Description,
                    CreatedAt = t.CreatedAt,
                    RelatedRentalId = t.RentalSessionId
                })
                .ToListAsync();

            return new WalletInfoDto
            {
                Balance = user?.WalletBalance ?? 0,
                RecentTransactions = transactions
            };
        }

        public async Task<(bool IsSuccess, string Message, WalletInfoDto? Data)> TopUpAsync(int userId, TopUpRequestDto request)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return (false, "User not found.", null);

            user.WalletBalance += request.Amount;

            var transaction = new WalletTransaction
            {
                UserId = userId,
                Amount = request.Amount,
                Type = TransactionType.TopUp,
                Description = $"Wallet Top-up of {request.Amount:C}",
                CreatedAt = DateTime.UtcNow
            };

            _context.WalletTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            var info = await GetWalletInfoAsync(userId);
            return (true, $"Successfully added {request.Amount:C} to your wallet.", info);
        }
    }
}
