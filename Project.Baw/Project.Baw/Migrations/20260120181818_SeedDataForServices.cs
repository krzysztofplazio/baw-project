using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project.Baw.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataForServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "services",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("097c35ce-592f-46e5-b3aa-152471521f17"), "Gardening", 50.00m },
                    { new Guid("504ae01b-230d-4556-a17c-e1d59acd01c1"), "Plumbing", 40.00m },
                    { new Guid("963a2269-cf14-4f59-bd14-6bce7620f941"), "Carpentry", 30.00m },
                    { new Guid("b9fb52b0-f437-40df-9e84-634a25e37736"), "Painting", 20.00m },
                    { new Guid("c292215d-bafa-4ce4-9c45-1bd0ff54623e"), "Cleaning", 10.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "services",
                keyColumn: "Id",
                keyValue: new Guid("097c35ce-592f-46e5-b3aa-152471521f17"));

            migrationBuilder.DeleteData(
                table: "services",
                keyColumn: "Id",
                keyValue: new Guid("504ae01b-230d-4556-a17c-e1d59acd01c1"));

            migrationBuilder.DeleteData(
                table: "services",
                keyColumn: "Id",
                keyValue: new Guid("963a2269-cf14-4f59-bd14-6bce7620f941"));

            migrationBuilder.DeleteData(
                table: "services",
                keyColumn: "Id",
                keyValue: new Guid("b9fb52b0-f437-40df-9e84-634a25e37736"));

            migrationBuilder.DeleteData(
                table: "services",
                keyColumn: "Id",
                keyValue: new Guid("c292215d-bafa-4ce4-9c45-1bd0ff54623e"));
        }
    }
}
