using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeRental.API.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedDataStaticDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // This migration exists to fix the PendingModelChangesWarning caused by
            // DateTime.UtcNow being used in HasData() seed calls (dynamic values).
            // The actual seed data timestamps are already correct in the DB.
            // No schema changes are needed here — this is intentionally a no-op.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No-op
        }
    }
}
