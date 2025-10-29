using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class SetNullDeleteBehaviorCommissionToEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Commissions_CommissionId",
                table: "Enrollments");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Commissions_CommissionId",
                table: "Enrollments",
                column: "CommissionId",
                principalTable: "Commissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Commissions_CommissionId",
                table: "Enrollments");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Commissions_CommissionId",
                table: "Enrollments",
                column: "CommissionId",
                principalTable: "Commissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
