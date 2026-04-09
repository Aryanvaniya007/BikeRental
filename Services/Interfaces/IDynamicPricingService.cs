using BikeRental.Models;

namespace BikeRental.Services.Interfaces
{
    public interface IDynamicPricingService
    {
        decimal CalculateAmount(RentalSession rental);
        decimal EstimateCost(Bike bike, int estimatedHours, bool isWeekend, bool isPeakHour);
    }
}
