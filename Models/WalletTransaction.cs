using System;

namespace BikeRental.Models
{
    public enum TransactionType
    {
        TopUp,      // User added money
        RentalPaid, // Deduction for a trip
        Refund      // Admin refund
    }

    /// <summary>
    /// Represents a financial transaction in the user's wallet.
    /// </summary>
    public class WalletTransaction
    {
        public int WalletTransactionId { get; set; }
        
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public decimal Amount { get; set; }
        
        public TransactionType Type { get; set; }

        /// <summary>
        /// Optional reference to a rental session if the transaction is a payment.
        /// </summary>
        public int? RentalSessionId { get; set; }
        public virtual RentalSession? RentalSession { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
