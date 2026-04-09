using BikeRental.DTOs.Requests;
using BikeRental.DTOs.Responses;
using BikeRental.Infrastructure.Data;
using BikeRental.Models;
using BikeRental.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Services
{
    public class RentalService : IRentalService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDynamicPricingService _pricingService;

        public RentalService(ApplicationDbContext context, IDynamicPricingService pricingService)
        {
            _context = context;
            _pricingService = pricingService;
        }

        public async Task<(bool IsSuccess, string Message, RideStartedDto? Data)> StartRideAsync(int userId, StartRideDto request)
        {
            var bike = await _context.Bikes.FindAsync(request.BikeId);
            if (bike == null) return (false, "Bike not found.", null);

            if (bike.Status != BikeStatus.Available)
                return (false, "Bike is not available.", null);

            var activeRental = await _context.RentalSessions
                .FirstOrDefaultAsync(r => r.UserId == userId && r.Status == RentalStatus.Active);
            if (activeRental != null)
                return (false, "User already has an active rental.", null);

            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.WalletBalance < bike.HourlyRate)
                return (false, $"Insufficient balance. Minimum {bike.HourlyRate:C} required to start ride.", null);

            var session = new RentalSession
            {
                UserId = userId,
                BikeId = request.BikeId,
                StartTime = DateTime.UtcNow,
                Status = RentalStatus.Active
            };

            bike.Status = BikeStatus.InUse;
            bike.CurrentRental = session;

            _context.RentalSessions.Add(session);
            await _context.SaveChangesAsync();

            var data = new RideStartedDto
            {
                RentalId = session.RentalId,
                StartTime = session.StartTime,
                Bike = new BikeInfoDto
                {
                    BikeId = bike.BikeId,
                    Model = bike.Model,
                    Status = bike.Status.ToString(),
                    Location = bike.Location,
                    HourlyRate = bike.HourlyRate,
                    DailyRate = bike.DailyRate
                }
            };

            return (true, "Ride started", data);
        }

        public async Task<(bool IsSuccess, string Message, RideEndedDto? Data)> EndRideAsync(int userId, EndRideDto request)
        {
            var session = await _context.RentalSessions
                .Include(r => r.Bike)
                .FirstOrDefaultAsync(r => r.RentalId == request.RentalId && r.UserId == userId);

            if (session == null) return (false, "Rental not found or unauthorized.", null);
            if (session.Status != RentalStatus.Active) return (false, "Rental is already completed or cancelled.", null);

            session.EndTime = DateTime.UtcNow;
            session.Status = RentalStatus.Completed;

            session.Bike.Status = BikeStatus.Available;
            session.Bike.CurrentRentalId = null;

            try
            {
                session.TotalAmount = _pricingService.CalculateAmount(session);
            }
            catch (InvalidOperationException ex)
            {
                return (false, ex.Message, null);
            }

            var payment = new Payment
            {
                RentalId = session.RentalId,
                Amount = session.TotalAmount.Value,
                PaymentMethod = "Wallet",
                Status = PaymentStatus.Completed, 
                TransactionId = Guid.NewGuid().ToString()
            };
            
            _context.Payments.Add(payment);

            // Deduct from User Wallet
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.WalletBalance -= session.TotalAmount.Value;
                
                // Add Wallet Transaction
                _context.WalletTransactions.Add(new WalletTransaction
                {
                    UserId = userId,
                    Amount = -session.TotalAmount.Value,
                    Type = TransactionType.RentalPaid,
                    RentalSessionId = session.RentalId,
                    Description = $"Payment for rental #{session.RentalId} (Bike: {session.Bike.Model})",
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();

            session.PaymentId = payment.PaymentId;
            await _context.SaveChangesAsync();

            var data = new RideEndedDto
            {
                RentalId = session.RentalId,
                DurationHours = Math.Round((session.EndTime.Value - session.StartTime).TotalHours, 2),
                TotalAmount = session.TotalAmount.Value,
                Payment = new PaymentInfoDto
                {
                    PaymentId = payment.PaymentId,
                    Amount = payment.Amount,
                    PaymentMethod = payment.PaymentMethod,
                    TransactionId = payment.TransactionId,
                    Status = payment.Status.ToString()
                }
            };

            return (true, "Ride ended", data);
        }

        public async Task<IEnumerable<ActiveRentalDto>> GetActiveRentalsAsync()
        {
            return await _context.RentalSessions
                .Include(r => r.User)
                .Include(r => r.Bike)
                .Where(r => r.Status == RentalStatus.Active)
                .Select(r => new ActiveRentalDto
                {
                    RentalId = r.RentalId,
                    UserId = r.UserId,
                    UserName = r.User.FullName ?? "N/A",
                    UserPhone = r.User.PhoneNumber,
                    BikeId = r.BikeId,
                    BikeModel = r.Bike.Model,
                    StartTime = r.StartTime,
                    DurationHours = Math.Round((DateTime.UtcNow - r.StartTime).TotalHours, 2),
                    EstimatedAmount = null
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<RentalHistoryDto>> GetUserRentalsAsync(int userId)
        {
            return await _context.RentalSessions
                .Include(r => r.Bike)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.StartTime)
                .Select(r => new RentalHistoryDto
                {
                    RentalId = r.RentalId,
                    BikeId = r.BikeId,
                    BikeModel = r.Bike.Model,
                    StartTime = r.StartTime,
                    EndTime = r.EndTime,
                    TotalAmount = r.TotalAmount,
                    Status = r.Status.ToString()
                })
                .ToListAsync();
        }
    }
}
