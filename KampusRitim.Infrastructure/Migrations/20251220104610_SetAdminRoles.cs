using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KampusRitim.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetAdminRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE ""Users"" SET ""Role"" = 1 WHERE ""Email"" IN ('yonetici@dogus.edu.tr', 'admin@dogus.edu.tr');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE ""Users"" SET ""Role"" = 0 WHERE ""Email"" IN ('yonetici@dogus.edu.tr', 'admin@dogus.edu.tr');");
        }
    }
}
