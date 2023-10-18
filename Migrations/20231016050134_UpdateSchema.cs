using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodePulse.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoriesId",
                table: "BlogPost",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPost_CategoriesId",
                table: "BlogPost",
                column: "CategoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPost_Categories_CategoriesId",
                table: "BlogPost",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPost_Categories_CategoriesId",
                table: "BlogPost");

            migrationBuilder.DropIndex(
                name: "IX_BlogPost_CategoriesId",
                table: "BlogPost");

            migrationBuilder.DropColumn(
                name: "CategoriesId",
                table: "BlogPost");
        }
    }
}
