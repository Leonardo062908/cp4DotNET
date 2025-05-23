using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "RM554889");

            migrationBuilder.CreateTable(
                name: "MOTO",
                schema: "RM554889",
                columns: table => new
                {
                    ID_MOTO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    MODELO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    PLACA = table.Column<string>(type: "NVARCHAR2(7)", maxLength: 7, nullable: false),
                    STATUSMOTO_ID_STATUS = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTO", x => x.ID_MOTO);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MOTO",
                schema: "RM554889");
        }
    }
}
