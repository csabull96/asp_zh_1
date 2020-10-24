using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheAdvertiser.Data.Migrations
{
    public partial class PhotoUpload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Advertisements",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "Advertisements",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Advertisements");
        }
    }
}
