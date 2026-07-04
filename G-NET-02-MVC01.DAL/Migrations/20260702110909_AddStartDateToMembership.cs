using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G_NET_02_MVC01.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddStartDateToMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate1",
                table: "Membership",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate1",
                table: "Membership");
        }
    }
}
