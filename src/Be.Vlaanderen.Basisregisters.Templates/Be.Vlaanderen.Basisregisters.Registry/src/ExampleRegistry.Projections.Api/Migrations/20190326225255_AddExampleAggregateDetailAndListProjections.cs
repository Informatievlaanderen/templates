using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExampleRegistry.Projections.Api.Migrations
{
    public partial class AddExampleAggregateDetailAndListProjections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Api");

            migrationBuilder.CreateTable(
                name: "ExampleAggregateDetails",
                schema: "Api",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NameDutch = table.Column<string>(maxLength: 200, nullable: true),
                    NameFrench = table.Column<string>(maxLength: 200, nullable: true),
                    NameEnglish = table.Column<string>(maxLength: 200, nullable: true),
                    NameGerman = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleAggregateDetails", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "ExampleAggregateList",
                schema: "Api",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleAggregateList", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "ProjectionStates",
                schema: "Api",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Position = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectionStates", x => x.Name)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExampleAggregateDetails_NameDutch",
                schema: "Api",
                table: "ExampleAggregateDetails",
                column: "NameDutch")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_ExampleAggregateList_Name",
                schema: "Api",
                table: "ExampleAggregateList",
                column: "Name")
                .Annotation("SqlServer:Clustered", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExampleAggregateDetails",
                schema: "Api");

            migrationBuilder.DropTable(
                name: "ExampleAggregateList",
                schema: "Api");

            migrationBuilder.DropTable(
                name: "ProjectionStates",
                schema: "Api");
        }
    }
}
