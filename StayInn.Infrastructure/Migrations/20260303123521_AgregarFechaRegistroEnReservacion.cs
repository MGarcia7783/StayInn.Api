using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayInn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFechaRegistroEnReservacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaRegistro",
                table: "Reservaciones",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "Reservaciones");
        }
    }
}
