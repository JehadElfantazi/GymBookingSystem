using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersionToClassSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "ClassSchedules",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ClassSchedules");
        }
    }
}
