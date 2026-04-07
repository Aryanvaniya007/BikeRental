using System;
using BikeRental.Models;

namespace BikeRental.Services
{
    /// <summary>
    /// Dynamic pricing engine that calculates rental costs based on:
    /// - Base rates (hourly/daily)
    /// - Peak hour multipliers
    /// - Weekend multipliers
    /// - Duration (hours or days)
    /// </summary>
    public static class DynamicPricingEngine
    {
        // Peak hour windows (local time)
        private static readonly (int start, int end) MorningPeak = (8, 10);
        private static readonly (int start, int end) EveningPeak = (17, 20);

        /// <summary>
        /// Calculates the total rental amount based on dynamic pricing rules.
        /// </summary>
        /// <param name="rental">The completed rental session with EndTime set</param>
        /// <returns>The final amount to be charged</returns>
        /// <exception cref="InvalidOperationException">Thrown when EndTime is not set</exception>
        public static decimal CalculateAmount(RentalSession rental)
        {
            if (rental.EndTime == null)
            {
                throw new InvalidOperationException("Cannot calculate amount for an active rental. EndTime must be set.");
            }

            var duration = rental.EndTime.Value - rental.StartTime;
            decimal baseHourlyRate = rental.Bike.HourlyRate;
            decimal multiplier = 1.0m;

            // Determine peak hour multiplier
            // Check if rental started during peak hours
            var startHour = rental.StartTime.Hour;
            var isPeakHour = IsPeakHour(startHour);

            if (isPeakHour)
            {
                multiplier *= 1.5m;
            }

            // Check weekend multiplier
            var startDay = rental.StartTime.DayOfWeek;
            var isWeekend = startDay == DayOfWeek.Saturday || startDay == DayOfWeek.Sunday;

            if (isWeekend)
            {
                multiplier *= 1.3m;
            }

            // Duration-based pricing: > 24 hours = daily rate
            if (duration.TotalHours >= 24)
            {
                var days = Math.Ceiling(duration.TotalHours / 24);
                var dailyTotal = rental.Bike.DailyRate * days;
                return Math.Round(dailyTotal * multiplier, 2);
            }
            else
            {
                // Hourly pricing with minimum 1 hour
                var hours = Math.Ceiling(duration.TotalHours);
                if (hours < 1) hours = 1;

                var hourlyTotal = rental.Bike.HourlyRate * hours;
                return Math.Round(hourlyTotal * multiplier, 2);
            }
        }

        /// <summary>
        /// Checks if a given hour falls within peak hours.
        /// </summary>
        /// <param name="hour">Hour in 24-hour format (0-23)</param>
        /// <returns>True if within peak time window</returns>
        private static bool IsPeakHour(int hour)
        {
            return (hour >= MorningPeak.start && hour < MorningPeak.end) ||
                   (hour >= EveningPeak.start && hour < EveningPeak.end);
        }

        /// <summary>
        /// Estimates cost for a potential rental (before starting).
        /// Useful for displaying approximate pricing to users.
        /// </summary>
        /// <param name="bike">The bike to be rented</param>
        /// <param name="estimatedHours">Number of hours estimated</param>
        /// <param name="isWeekend">Whether rental would be on weekend</param>
        /// <param name="isPeakHour">Whether rental would start during peak hours</param>
        /// <returns>Estimated total cost</returns>
        public static decimal EstimateCost(Bike bike, int estimatedHours, bool isWeekend, bool isPeakHour)
        {
            decimal multiplier = 1.0m;
            if (isPeakHour) multiplier *= 1.5m;
            if (isWeekend) multiplier *= 1.3m;

            if (estimatedHours >= 24)
            {
                var days = Math.Ceiling(estimatedHours / 24.0);
                return Math.Round(bike.DailyRate * days * multiplier, 2);
            }
            else
            {
                var hours = estimatedHours < 1 ? 1 : estimatedHours;
                return Math.Round(bike.HourlyRate * hours * multiplier, 2);
            }
        }
    }
}
