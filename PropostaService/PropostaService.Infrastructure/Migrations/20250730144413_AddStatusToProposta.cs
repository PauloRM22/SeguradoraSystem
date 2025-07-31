using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropostaService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToProposta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Propostas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Propostas");
        }
    }
}
