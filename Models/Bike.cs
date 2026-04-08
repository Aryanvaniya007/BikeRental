using System;
using System.Collections.Generic;

namespace BikeRental.Models
{
    /// <summary>
    /// Enum representing the operational status of a bicycle.
    /// </summary>
    public enum BikeStatus
    {
        /// <summary>
        /// Bike is available for rental.
        /// </summary>
        Available = 0,

        /// <summary>
        /// Bike is currently rented by a user.
        /// </summary>
        InUse = 1,

        /// <summary>
        /// Bike is out of service for maintenance or repairs.
        /// </summary>
        Maintenance = 2
    }

    /// <summary>
    /// Represents a bicycle in the rental fleet.
    /// </summary>
    public class Bike
    {
        /// <summary>
        /// Primary key for the bike.
        /// </summary>
        public int BikeId { get; set; }

        /// <summary>
        /// Model/type of the bicycle (e.g., "Trek Marlin 7").
        /// </summary>
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Current operational status of the bike.
        /// </summary>
        public BikeStatus Status { get; set; } = BikeStatus.Available;

        /// <summary>
        /// Foreign key to the current rental session (null if available).
        /// Reference: RentalSessions(RentalId)
        /// </summary>
        public int? CurrentRentalId { get; set; }

        /// <summary>
        /// Physical location of the bike (station, shop, etc.).
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// Price per hour for renting this bike.
        /// Default: $5.00
        /// </summary>
        public decimal HourlyRate { get; set; } = 5.00m;

        /// <summary>
        /// Price per day (24 hours) for renting this bike.
        /// Default: $30.00
        /// </summary>
        public decimal DailyRate { get; set; } = 30.00m;

        /// <summary>
        /// Timestamp when the bike record was created (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when the bike record was last updated (UTC).
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // ============================================
        // Navigation Properties (Entity Framework)
        // ============================================

        /// <summary>
        /// The active rental session for this bike (if currently rented).
        /// </summary>
        public virtual RentalSession? CurrentRental { get; set; }

        /// <summary>
        /// Historical rental sessions for this bike (read-only).
        /// </summary>
        public virtual ICollection<RentalSession> RentalHistory { get; set; } = new List<RentalSession>();
    }
}
