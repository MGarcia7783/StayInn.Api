using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayInn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifcarTipoDatoTablaHabitacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "FechaSalida",
                table: "Reservaciones",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "FechaEntrada",
                table: "Reservaciones",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "FechaSalida",
                table: "Reservaciones",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "FechaEntrada",
                table: "Reservaciones",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
