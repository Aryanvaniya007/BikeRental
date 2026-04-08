using Microsoft.EntityFrameworkCore;
using BikeRental.Models;

namespace BikeRental.Infrastructure.Data
{
    /// <summary>
    /// Entity Framework DbContext for the Bike Rental Database.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ============================
        // DbSet Properties
        // ============================

        public DbSet<User> Users { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<RentalSession> RentalSessions { get; set; }
        public DbSet<Payment> Payments { get; set; }

        /// <summary>
        /// Configures entity relationships and constraints.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // User Entity Configuration
            // ==========================================
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(e => e.UserId);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.OTP)
                    .HasMaxLength(6);

                entity.HasIndex(e => e.PhoneNumber)
                    .IsUnique();

                entity.HasIndex(e => e.IsVerified);

                entity.HasMany(e => e.RentalSessions)
                    .WithOne(e => e.User)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

<<<<<<< HEAD
                    //entity.HasMany(e => e.Payments)
                    //    .WithOne(e => e.Rental)
                    //    .HasForeignKey(e => e.RentalId)
                    //    .OnDelete(DeleteBehavior.Cascade);
                });
=======
                entity.HasMany(e => e.Payments)
                    .WithOne(e => e.Rental)
                    .HasForeignKey(e => e.RentalId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
>>>>>>> ff3bf0158343136f073dc12483c35413107580d9

            // ==========================================
            // Bike Entity Configuration
            // ==========================================
            modelBuilder.Entity<Bike>(entity =>
            {
                entity.ToTable("Bikes");

                entity.HasKey(e => e.BikeId);

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Location)
                    .HasMaxLength(255);

                entity.Property(e => e.HourlyRate)
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.DailyRate)
                    .HasColumnType("decimal(10,2)");

                entity.HasIndex(e => e.Status);

                entity.HasOne(e => e.CurrentRental)
                    .WithOne()
                    .HasForeignKey<Bike>(e => e.CurrentRentalId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(e => e.RentalHistory)
                    .WithOne(e => e.Bike)
                    .HasForeignKey(e => e.BikeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ==========================================
            // RentalSession Entity Configuration
            // ==========================================
            modelBuilder.Entity<RentalSession>(entity =>
            {
                entity.ToTable("RentalSessions");

                entity.HasKey(e => e.RentalId);

                entity.Property(e => e.Status)
                    .HasConversion<int>(); // Store enum as integer

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.BikeId);
                entity.HasIndex(e => e.Status);

                entity.HasOne(e => e.User)
                    .WithMany(e => e.RentalSessions)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Bike)
                    .WithMany(e => e.RentalHistory)
                    .HasForeignKey(e => e.BikeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Payment)
                    .WithOne()
                    .HasForeignKey<RentalSession>(e => e.PaymentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ==========================================
            // Payment Entity Configuration
            // ==========================================
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payments");

                entity.HasKey(e => e.PaymentId);

                entity.Property(e => e.RentalId)
                    .ValueGeneratedNever(); // Same as RentalId (1:1)

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(50)
                    .HasDefaultValue("Wallet");

                entity.Property(e => e.TransactionId)
                    .HasMaxLength(100);

                entity.Property(e => e.Status)
                    .HasConversion<int>(); // Store enum as integer

                entity.HasIndex(e => e.RentalId)
                    .IsUnique();

                entity.HasIndex(e => e.Status);

                entity.HasOne(e => e.Rental)
                    .WithOne(e => e.Payment)
                    .HasForeignKey<Payment>(e => e.RentalId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ==========================================
            // Configure Enum Conversions
            // ==========================================
            modelBuilder.Entity<Bike>()
                .Property(e => e.Status)
                .HasConversion<int>();

            modelBuilder.Entity<RentalSession>()
                .Property(e => e.Status)
                .HasConversion<int>();

            modelBuilder.Entity<Payment>()
                .Property(e => e.Status)
                .HasConversion<int>();

            // ==========================================
            // Seed Initial Data
            // ==========================================
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    PhoneNumber = "+1234567890",
                    IsVerified = true,
                    IsAdmin = true,
                    FullName = "System Admin",
                    Email = "admin@bikerental.com",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    UserId = 2,
                    PhoneNumber = "+0987654321",
                    IsVerified = true,
                    IsAdmin = false,
                    FullName = "John Doe",
                    Email = "john@example.com",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            modelBuilder.Entity<Bike>().HasData(
                new Bike
                {
                    BikeId = 1,
                    Model = "Trek Marlin 7",
                    Status = BikeStatus.Available,
                    Location = "Central Station",
                    HourlyRate = 6.00m,
                    DailyRate = 35.00m,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Bike
                {
                    BikeId = 2,
                    Model = "Giant Escape 3",
                    Status = BikeStatus.Available,
                    Location = "City Park",
                    HourlyRate = 5.00m,
                    DailyRate = 28.00m,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Bike
                {
                    BikeId = 3,
                    Model = "Specialized Sirrus",
                    Status = BikeStatus.Available,
                    Location = "Harbor Point",
                    HourlyRate = 7.00m,
                    DailyRate = 40.00m,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
