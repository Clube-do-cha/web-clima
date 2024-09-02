using Microsoft.EntityFrameworkCore.Migrations;

public partial class UpdateUserModel : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "FullName",
            table: "AspNetUsers",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "UserLogin",
            table: "AspNetUsers",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "UserCity",
            table: "AspNetUsers",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<byte[]>(
            name: "UserProfilePic",
            table: "AspNetUsers",
            type: "varbinary(max)",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "FullName", table: "AspNetUsers");
        migrationBuilder.DropColumn(name: "UserLogin", table: "AspNetUsers");
        migrationBuilder.DropColumn(name: "UserCity", table: "AspNetUsers");
        migrationBuilder.DropColumn(name: "UserProfilePic", table: "AspNetUsers");
    }
}
