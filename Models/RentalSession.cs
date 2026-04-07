using System;
using System.Collections.Generic;

namespace BikeRental.Models
{
    /// <summary>
    /// Enum representing the lifecycle status of a rental session.
    /// </summary>
    public enum RentalStatus
    {
        /// <summary>
        /// Rental is currently active (bike is with user).
        /// </summary>
        Active = 0,

        /// <summary>
        /// Rental has been completed and payment received.
        /// </summary>
        Completed = 1,

        /// <summary>
        /// Rental was cancelled before completion (refund may apply).
        /// </summary>
        Cancelled = 2
    }

    /// <summary>
    /// Represents a rental transaction from start to end.
    /// Tracks which user rented which bike, duration, and payment details.
    /// </summary>
    public class RentalSession
    {
        /// <summary>
        /// Primary key for the rental session.
        /// </summary>
        public int RentalId { get; set; }

        /// <summary>
        /// Foreign key to the user who rented the bike.
        /// Reference: Users(UserId)
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Foreign key to the rented bike.
        /// Reference: Bikes(BikeId)
        /// </summary>
        public int BikeId { get; set; }

        /// <summary>
        /// Timestamp when the rental started (UTC).
        /// Set automatically on creation.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when the rental ended (UTC).
        /// Null while rental is active.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Total amount charged for the rental (calculated on end).
        /// Null until rental is completed.
        /// </summary>
        public decimal? TotalAmount { get; set; }

        /// <summary>
        /// Foreign key to the associated payment record.
        /// Reference: Payments(PaymentId)
        /// </summary>
        public int? PaymentId { get; set; }

        /// <summary>
        /// Current status of the rental session.
        /// </summary>
        public RentalStatus Status { get; set; } = RentalStatus.Active;

        /// <summary>
        /// Timestamp when the rental record was created (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ============================================
        // Navigation Properties (Entity Framework)
        // ============================================

        /// <summary>
        /// The user who rented the bike (required).
        /// </summary>
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// The bike that was rented (required).
        /// </summary>
        public virtual Bike Bike { get; set; } = null!;

        /// <summary>
        /// The payment record for this rental (optional, set on completion).
        /// </summary>
        public virtual Payment? Payment { get; set; }
    }
}
