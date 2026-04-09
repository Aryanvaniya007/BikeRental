using System;
using System.Collections.Generic;

namespace BikeRental.Models
{
<<<<<<< HEAD
=======
<<<<<<< HEAD
    /// Enum representing the status of a payment transaction.
    public enum PaymentStatus
    {
        Pending = 0,

        Completed = 1,

        Failed = 2
    }

    
    /// Represents a payment transaction for a rental session.
    /// Stores payment method, amount, status, and gateway response.
    
    public class Payment
    {
        public int PaymentId { get; set; }

        public int RentalId { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = "Wallet";

        public string? TransactionId { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public string? GatewayResponse { get; set; }

      
        /// Timestamp when the payment record was created (UTC).
      
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
      
        /// The rental session associated with this payment (required).
        
=======
>>>>>>> 9d1ea28f7fb269cff41e45279fc921029eb77566
    /// <summary>
    /// Enum representing the status of a payment transaction.
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// Payment is pending processing.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Payment has been successfully completed.
        /// </summary>
        Completed = 1,

        /// <summary>
        /// Payment failed or was declined.
        /// </summary>
        Failed = 2
    }

    /// <summary>
    /// Represents a payment transaction for a rental session.
    /// Stores payment method, amount, status, and gateway response.
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// Primary key for the payment record.
        /// </summary>
        public int PaymentId { get; set; }

        /// <summary>
        /// Foreign key to the rental session (unique - one-to-one relationship).
        /// Reference: RentalSessions(RentalId)
        /// </summary>
        public int RentalId { get; set; }

        /// <summary>
        /// Amount charged for the rental (in USD or local currency).
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Payment method used: "Wallet", "Card", "UPI", etc.
        /// Default: "Wallet"
        /// </summary>
        public string PaymentMethod { get; set; } = "Wallet";

        /// <summary>
        /// Transaction ID from the payment gateway (if applicable).
        /// </summary>
        public string? TransactionId { get; set; }

        /// <summary>
        /// Current status of the payment.
        /// </summary>
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        /// <summary>
        /// Raw response or error message from the payment gateway.
        /// Useful for debugging failed payments.
        /// </summary>
        public string? GatewayResponse { get; set; }

        /// <summary>
        /// Timestamp when the payment record was created (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ============================================
        // Navigation Properties (Entity Framework)
        // ============================================

        /// <summary>
        /// The rental session associated with this payment (required).
        /// </summary>
<<<<<<< HEAD
=======
>>>>>>> ff3bf0158343136f073dc12483c35413107580d9
>>>>>>> 9d1ea28f7fb269cff41e45279fc921029eb77566
        public virtual RentalSession Rental { get; set; } = null!;
    }
}
