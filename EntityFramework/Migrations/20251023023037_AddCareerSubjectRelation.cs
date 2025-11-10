using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddCareerSubjectRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareerSubject_Careers_CareersId",
                table: "CareerSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_CareerSubject_Subjects_SubjectsId",
                table: "CareerSubject");

            migrationBuilder.RenameColumn(
                name: "SubjectsId",
                table: "CareerSubject",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "CareersId",
                table: "CareerSubject",
                newName: "CareerId");

            migrationBuilder.RenameIndex(
                name: "IX_CareerSubject_SubjectsId",
                table: "CareerSubject",
                newName: "IX_CareerSubject_SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_CareerSubject_Careers_CareerId",
                table: "CareerSubject",
                column: "CareerId",
                principalTable: "Careers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CareerSubject_Subjects_SubjectId",
                table: "CareerSubject",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareerSubject_Careers_CareerId",
                table: "CareerSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_CareerSubject_Subjects_SubjectId",
                table: "CareerSubject");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "CareerSubject",
                newName: "SubjectsId");

            migrationBuilder.RenameColumn(
                name: "CareerId",
                table: "CareerSubject",
                newName: "CareersId");

            migrationBuilder.RenameIndex(
                name: "IX_CareerSubject_SubjectId",
                table: "CareerSubject",
                newName: "IX_CareerSubject_SubjectsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CareerSubject_Careers_CareersId",
                table: "CareerSubject",
                column: "CareersId",
                principalTable: "Careers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CareerSubject_Subjects_SubjectsId",
                table: "CareerSubject",
                column: "SubjectsId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
