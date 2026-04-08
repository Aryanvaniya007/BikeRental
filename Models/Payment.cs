using System;
using System.Collections.Generic;

namespace BikeRental.Models
{
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
        
        public virtual RentalSession Rental { get; set; } = null!;
    }
}
