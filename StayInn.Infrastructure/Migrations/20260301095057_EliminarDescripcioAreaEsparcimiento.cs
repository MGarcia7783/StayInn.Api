using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayInn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EliminarDescripcioAreaEsparcimiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "AreasEsparcimiento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "AreasEsparcimiento",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
