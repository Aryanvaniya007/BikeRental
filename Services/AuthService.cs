using BikeRental.DTOs.Requests;
using BikeRental.DTOs.Responses;
using BikeRental.Infrastructure.Data;
using BikeRental.Models;
using BikeRental.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BikeRental.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<(bool IsSuccess, string Message, string? DemoOtp)> RequestOtpAsync(RequestOtpDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
            
            if (user == null)
            {
                user = new User
                {
                    PhoneNumber = request.PhoneNumber,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsVerified = false,
                    IsAdmin = false
                };
                _context.Users.Add(user);
            }

            var otp = Random.Shared.Next(100000, 999999).ToString();
            user.OTP = otp;
            user.OTPExpiry = DateTime.UtcNow.AddMinutes(5);
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return (true, $"OTP sent to {request.PhoneNumber}", otp);
        }

        public async Task<(bool IsSuccess, string Message, AuthResponseDto? Data)> VerifyOtpAsync(VerifyOtpDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
            
            if (user == null || user.OTP != request.Otp || user.OTPExpiry < DateTime.UtcNow)
            {
                return (false, "Invalid or expired OTP.", null);
            }

            user.IsVerified = true;
            user.OTP = null;
            user.OTPExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            
            var response = new AuthResponseDto
            {
                Token = token,
                UserId = user.UserId,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                IsAdmin = user.IsAdmin
            };

            return (true, "Success", response);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is missing."));
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
            };

            if (user.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["DurationInMinutes"] ?? "60")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
