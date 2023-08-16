using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tareas_mvc.Migrations
{
    /// <inheritdoc />
    public partial class AdminRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT Id FROM AspNetRoles WHERE Id = '02075185-9020-4ec1-a568-b25a00e8fe33')
                                    BEGIN
	                                    INSERT AspNetRoles (Id, Name, NormalizedName)
	                                    VALUES ('02075185-9020-4ec1-a568-b25a00e8fe33', 'admin', 'ADMIN')
                                    END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE AspNetRoleClaims WHERE Id = '02075185-9020-4ec1-a568-b25a00e8fe33'");
        }
    }
}
