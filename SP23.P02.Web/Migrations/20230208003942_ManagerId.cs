﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SP23.P02.Web.Migrations
{
    /// <inheritdoc />
    public partial class ManagerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TrainStation_ManagerId",
                table: "TrainStation",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainStation_AspNetUsers_ManagerId",
                table: "TrainStation",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainStation_AspNetUsers_ManagerId",
                table: "TrainStation");

            migrationBuilder.DropIndex(
                name: "IX_TrainStation_ManagerId",
                table: "TrainStation");
        }
    }
}
