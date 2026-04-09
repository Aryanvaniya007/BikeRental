using BikeRental.DTOs.Requests;
using BikeRental.DTOs.Responses;
using BikeRental.Infrastructure.Data;
using BikeRental.Models;
using BikeRental.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Services
{
    public class BikeService : IBikeService
    {
        private readonly ApplicationDbContext _context;

        public BikeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BikeInfoDto>> GetBikesAsync()
        {
            return await _context.Bikes
                .Select(b => new BikeInfoDto
                {
                    BikeId = b.BikeId,
                    Model = b.Model,
                    Status = b.Status.ToString(),
                    Location = b.Location,
                    HourlyRate = b.HourlyRate,
                    DailyRate = b.DailyRate,
                    ContactNumber = b.ContactNumber
                })
                .ToListAsync();
        }

        public async Task<(bool IsSuccess, string Message, MaintenanceReportDto? Data)> ReportMaintenanceAsync(int bikeId, ReportMaintenanceDto request)
        {
            var bike = await _context.Bikes.FindAsync(bikeId);
            if (bike == null) return (false, "Bike not found.", null);

            bike.Status = Models.BikeStatus.Maintenance;
            bike.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var data = new MaintenanceReportDto
            {
                Message = "Maintenance reported successfully.",
                BikeId = bike.BikeId,
                NewStatus = bike.Status.ToString(),
                ReportedAt = DateTime.UtcNow
            };

            return (true, "Success", data);
        }

        public async Task<(bool IsSuccess, string Message, BikeInfoDto? Data)> CreateBikeAsync(CreateBikeDto request)
        {
            var bike = new Bike
            {
                Model = request.Model,
                Status = BikeStatus.Available,
                Location = request.Location,
                HourlyRate = request.HourlyRate,
                DailyRate = request.DailyRate,
                ContactNumber = request.ContactNumber,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Bikes.Add(bike);
            await _context.SaveChangesAsync();

            var result = new BikeInfoDto
            {
                BikeId = bike.BikeId,
                Model = bike.Model,
                Status = bike.Status.ToString(),
                Location = bike.Location,
                HourlyRate = bike.HourlyRate,
                DailyRate = bike.DailyRate,
                ContactNumber = bike.ContactNumber
            };

            return (true, "Bike created successfully.", result);
        }

        public async Task<(bool IsSuccess, string Message, BikeInfoDto? Data)> UpdateBikeAsync(int bikeId, UpdateBikeDto request)
        {
            var bike = await _context.Bikes.FindAsync(bikeId);
            if (bike == null) return (false, "Bike not found.", null);

            bike.Model = request.Model;
            bike.Location = request.Location;
            bike.HourlyRate = request.HourlyRate;
            bike.DailyRate = request.DailyRate;
            bike.ContactNumber = request.ContactNumber;
            bike.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new BikeInfoDto
            {
                BikeId = bike.BikeId,
                Model = bike.Model,
                Status = bike.Status.ToString(),
                Location = bike.Location,
                HourlyRate = bike.HourlyRate,
                DailyRate = bike.DailyRate,
                ContactNumber = bike.ContactNumber
            };

            return (true, "Bike updated successfully.", result);
        }

        public async Task<(bool IsSuccess, string Message)> DeleteBikeAsync(int bikeId)
        {
            var bike = await _context.Bikes.Include(b => b.RentalHistory).FirstOrDefaultAsync(b => b.BikeId == bikeId);
            if (bike == null) return (false, "Bike not found.");

            if (bike.Status == BikeStatus.InUse)
            {
                return (false, "Cannot delete a bike that is currently in use.");
            }

            // To safely complete the deletion process without FK constraint violations,
            // we cascade delete any historical rental sessions associated with this bike.
            if (bike.RentalHistory.Any())
            {
                // Payments are also automatically managed via DeleteBehavior.Cascade on the RentalSession.
                _context.RentalSessions.RemoveRange(bike.RentalHistory);
                await _context.SaveChangesAsync(); // Delete children first to satisfy EF Core Restrict behavior
            }

            _context.Bikes.Remove(bike);
            await _context.SaveChangesAsync();

            return (true, "Bike deleted successfully.");
        }
    }
}
