using BikeRental.Models;
using BikeRental.Services.Interfaces;

namespace BikeRental.Services
{
    public class DynamicPricingService : IDynamicPricingService
    {
        private readonly (int start, int end) _morningPeak = (8, 10);
        private readonly (int start, int end) _eveningPeak = (17, 20);

        public decimal CalculateAmount(RentalSession rental)
        {
            if (rental.EndTime == null)
            {
                throw new InvalidOperationException("Cannot calculate amount for an active rental. EndTime must be set.");
            }

            var duration = rental.EndTime.Value - rental.StartTime;
            decimal multiplier = 1.0m;

            var startHour = rental.StartTime.Hour;
            var isPeakHour = IsPeakHour(startHour);

            if (isPeakHour) multiplier *= 1.5m;

            var startDay = rental.StartTime.DayOfWeek;
            var isWeekend = startDay == DayOfWeek.Saturday || startDay == DayOfWeek.Sunday;

            if (isWeekend) multiplier *= 1.3m;

            if (duration.TotalHours >= 24)
            {
                var days = Math.Ceiling(duration.TotalHours / 24);
                var dailyTotal = rental.Bike.DailyRate * (decimal)days;
                return Math.Round(dailyTotal * multiplier, 2);
            }
            else
            {
                var hours = Math.Ceiling(duration.TotalHours);
                if (hours < 1) hours = 1;

                var hourlyTotal = rental.Bike.HourlyRate * (decimal)hours;
                return Math.Round(hourlyTotal * multiplier, 2);
            }
        }

        public decimal EstimateCost(Bike bike, int estimatedHours, bool isWeekend, bool isPeakHour)
        {
            decimal multiplier = 1.0m;
            if (isPeakHour) multiplier *= 1.5m;
            if (isWeekend) multiplier *= 1.3m;

            if (estimatedHours >= 24)
            {
                var days = Math.Ceiling(estimatedHours / 24.0);
                return Math.Round(bike.DailyRate * (decimal)days * multiplier, 2);
            }
            else
            {
                var hours = estimatedHours < 1 ? 1 : estimatedHours;
                return Math.Round(bike.HourlyRate * hours * multiplier, 2);
            }
        }

        private bool IsPeakHour(int hour)
        {
            return (hour >= _morningPeak.start && hour < _morningPeak.end) ||
                   (hour >= _eveningPeak.start && hour < _eveningPeak.end);
        }
    }
}
