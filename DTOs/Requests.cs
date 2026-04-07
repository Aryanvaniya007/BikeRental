namespace BikeRental.DTOs.Requests
{
    /// <summary>
    /// Request DTO for OTP generation.
    /// </summary>
    public class RequestOtpDto
    {
        /// <summary>
        /// Phone number in international format (e.g., +1234567890).
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request DTO for OTP verification.
    /// </summary>
    public class VerifyOtpDto
    {
        /// <summary>
        /// Phone number in international format.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// 6-digit one-time password.
        /// </summary>
        public string Otp { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request DTO to start a new rental.
    /// </summary>
    public class StartRideDto
    {
        /// <summary>
        /// ID of the bike to be rented.
        /// </summary>
        public int BikeId { get; set; }
    }

    /// <summary>
    /// Request DTO to end an active rental.
    /// </summary>
    public class EndRideDto
    {
        /// <summary>
        /// ID of the rental session to end.
        /// </summary>
        public int RentalId { get; set; }
    }

    /// <summary>
    /// Request DTO to report maintenance for a bike.
    /// </summary>
    public class ReportMaintenanceDto
    {
        /// <summary>
        /// Reason for maintenance (e.g., "Flat tire", "Chain issue").
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// Optional detailed notes about the maintenance issue.
        /// </summary>
        public string? Notes { get; set; }
    }
}
