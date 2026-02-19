using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayInn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarRestrccionesTablaReservaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Reservacion_Estado",
                table: "Reservaciones");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Reservacion_Estado",
                table: "Reservaciones",
                sql: "\"Estado\" IN ('Pendiente','Confirmada', 'Activa', 'Finalizada','Cancelada')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Reservacion_Estado",
                table: "Reservaciones");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Reservacion_Estado",
                table: "Reservaciones",
                sql: "\"Estado\" IN ('Pendiente','Confirmada','Cancelada','Completada')");
        }
    }
}
