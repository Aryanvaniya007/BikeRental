using System;
using System.Collections.Generic;

namespace BikeRental.Models
{
    /// <summary>
    /// Represents a user in the bike rental system.
    /// Users can be either regular customers or administrators.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Primary key for the user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Unique phone number (used for authentication).
        /// Format: +[country code][number]
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// One-time password for verification (max 6 digits).
        /// </summary>
        public string? OTP { get; set; }

        /// <summary>
        /// Expiry timestamp for the OTP.
        /// </summary>
        public DateTime? OTPExpiry { get; set; }

        /// <summary>
        /// Indicates whether the user's phone number has been verified.
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Indicates whether the user has administrator privileges.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Full name of the user (optional).
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Email address (optional, for communications).
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Timestamp when the user record was created (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the current balance in the user's wallet.
        /// </summary>
        public decimal WalletBalance { get; set; } = 0.00m;

        /// <summary>
        /// Timestamp when the user record was last updated (UTC).
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // ============================================
        // Navigation Properties (Entity Framework)
        // ============================================

        /// <summary>
        /// Collection of rental sessions associated with this user.
        /// </summary>
        public virtual ICollection<RentalSession> RentalSessions { get; set; } = new List<RentalSession>();

    }
}
