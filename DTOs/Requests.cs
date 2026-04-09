using FluentValidation;

namespace BikeRental.DTOs.Requests
{
    public class RequestOtpDto
    {
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class RequestOtpDtoValidator : AbstractValidator<RequestOtpDto>
    {
        public RequestOtpDtoValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid international phone number format.");
        }
    }

    public class VerifyOtpDto
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }

    public class VerifyOtpDtoValidator : AbstractValidator<VerifyOtpDto>
    {
        public VerifyOtpDtoValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid international phone number format.");
                
            RuleFor(x => x.Otp)
                .NotEmpty().WithMessage("OTP is required.")
                .Length(6).WithMessage("OTP must be exactly 6 characters.");
        }
    }

    public class StartRideDto
    {
        public int BikeId { get; set; }
    }

    public class StartRideDtoValidator : AbstractValidator<StartRideDto>
    {
        public StartRideDtoValidator()
        {
            RuleFor(x => x.BikeId).GreaterThan(0).WithMessage("Valid Bike ID is required.");
        }
    }

    public class EndRideDto
    {
        public int RentalId { get; set; }
    }

    public class EndRideDtoValidator : AbstractValidator<EndRideDto>
    {
        public EndRideDtoValidator()
        {
            RuleFor(x => x.RentalId).GreaterThan(0).WithMessage("Valid Rental ID is required.");
        }
    }

    public class ReportMaintenanceDto
    {
        public string Reason { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    public class ReportMaintenanceDtoValidator : AbstractValidator<ReportMaintenanceDto>
    {
        public ReportMaintenanceDtoValidator()
        {
            RuleFor(x => x.Reason).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Notes).MaximumLength(500);
        }
    }

    public class CreateBikeDto
    {
        public string Model { get; set; } = string.Empty;
        public string? Location { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal DailyRate { get; set; }
        public string? ContactNumber { get; set; }
    }

    public class CreateBikeDtoValidator : AbstractValidator<CreateBikeDto>
    {
        public CreateBikeDtoValidator()
        {
            RuleFor(x => x.Model).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Location).MaximumLength(200);
            RuleFor(x => x.HourlyRate).GreaterThan(0).WithMessage("Hourly rate must be greater than 0");
            RuleFor(x => x.DailyRate).GreaterThan(0).WithMessage("Daily rate must be greater than 0");
            RuleFor(x => x.ContactNumber)
                .Matches(@"^\+?[1-9]\d{6,14}$")
                .WithMessage("Invalid phone number. Use international format e.g. +1234567890.")
                .When(x => !string.IsNullOrEmpty(x.ContactNumber));
        }
    }

    public class UpdateBikeDto
    {
        public string Model { get; set; } = string.Empty;
        public string? Location { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal DailyRate { get; set; }
        public string? ContactNumber { get; set; }
    }

    public class UpdateBikeDtoValidator : AbstractValidator<UpdateBikeDto>
    {
        public UpdateBikeDtoValidator()
        {
            RuleFor(x => x.Model).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Location).MaximumLength(200);
            RuleFor(x => x.HourlyRate).GreaterThan(0);
            RuleFor(x => x.DailyRate).GreaterThan(0);
            RuleFor(x => x.ContactNumber)
                .Matches(@"^\+?[1-9]\d{6,14}$")
                .WithMessage("Invalid phone number. Use international format e.g. +1234567890.")
                .When(x => !string.IsNullOrEmpty(x.ContactNumber));
        }
    }

    /// <summary>
    /// Request DTO for adding money to the wallet.
    /// </summary>
    public class TopUpRequestDto
    {
        public decimal Amount { get; set; }
    }

    public class TopUpRequestValidator : AbstractValidator<TopUpRequestDto>
    {
        public TopUpRequestValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
        }
    }
}
