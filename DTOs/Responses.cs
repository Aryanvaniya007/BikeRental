namespace BikeRental.DTOs.Responses
{
    /// <summary>
    /// Response DTO for successful OTP verification.
    /// Contains JWT token and user information.
    /// </summary>
    public class AuthResponseDto
    {
        /// <summary>
        /// JWT authentication token.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// User ID.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Full name of the user.
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Whether the user has admin privileges.
        /// </summary>
        public bool IsAdmin { get; set; }
    }

    /// <summary>
    /// Response DTO for starting a ride.
    /// </summary>
    public class RideStartedDto
    {
        /// <summary>
        /// ID of the created rental session.
        /// </summary>
        public int RentalId { get; set; }

        /// <summary>
        /// Timestamp when the ride started (UTC).
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Information about the rented bike.
        /// </summary>
        public BikeInfoDto Bike { get; set; } = new BikeInfoDto();
    }

    /// <summary>
    /// Response DTO for ending a ride.
    /// </summary>
    public class RideEndedDto
    {
        /// <summary>
        /// ID of the completed rental session.
        /// </summary>
        public int RentalId { get; set; }

        /// <summary>
        /// Duration of the rental in hours.
        /// </summary>
        public double DurationHours { get; set; }

        /// <summary>
        /// Total amount charged.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Payment information.
        /// </summary>
        public PaymentInfoDto? Payment { get; set; }
    }

    /// <summary>
    /// DTO for bike information (read-only).
    /// </summary>
    public class BikeInfoDto
    {
        public int BikeId { get; set; }
        public string Model { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Location { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal DailyRate { get; set; }
        public string? ContactNumber { get; set; }
    }

    /// <summary>
    /// DTO for payment information (read-only).
    /// </summary>
    public class PaymentInfoDto
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string? TransactionId { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response DTO for maintenance report.
    /// </summary>
    public class MaintenanceReportDto
    {
        public string Message { get; set; } = string.Empty;
        public int BikeId { get; set; }
        public string NewStatus { get; set; } = string.Empty;
        public DateTime ReportedAt { get; set; }
    }

    /// <summary>
    /// DTO for user information (admin views).
    /// </summary>
    public class UserInfoDto
    {
        public int UserId { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public bool IsVerified { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// DTO for active rental monitoring (admin).
    /// </summary>
    public class ActiveRentalDto
    {
        public int RentalId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
        public int BikeId { get; set; }
        public string BikeModel { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public double DurationHours { get; set; }
        public decimal? EstimatedAmount { get; set; }
    }

    /// <summary>
    /// DTO for a user's rental history.
    /// </summary>
    public class RentalHistoryDto
    {
        public int RentalId { get; set; }
        public int BikeId { get; set; }
        public string BikeModel { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for user wallet information.
    /// </summary>
    public class WalletInfoDto
    {
        public decimal Balance { get; set; }
        public IEnumerable<WalletTransactionDto> RecentTransactions { get; set; } = new List<WalletTransactionDto>();
    }

    /// <summary>
    /// DTO for wallet transactions.
    /// </summary>
    public class WalletTransactionDto
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int? RelatedRentalId { get; set; }
    }
}
