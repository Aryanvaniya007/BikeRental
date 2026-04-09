using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeRental.API.Migrations
{
    /// <inheritdoc />
    public partial class AddContactNumberToBike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The tables (Users, Bikes, RentalSessions, Payments) were created
            // outside of EF migrations. This migration only adds the new ContactNumber
            // column to Bikes. All CREATE TABLE / index / FK statements are skipped.

            // Ensure EF migrations history table exists (idempotent)
            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS ""__EFMigrationsHistory"" (
                    ""MigrationId"" character varying(150) NOT NULL,
                    ""ProductVersion"" character varying(32) NOT NULL,
                    CONSTRAINT ""PK___EFMigrationsHistory"" PRIMARY KEY (""MigrationId"")
                );
            ");

            // Add the new ContactNumber column (the actual schema change)
            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "Bikes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "Bikes");
        }
    }
}
