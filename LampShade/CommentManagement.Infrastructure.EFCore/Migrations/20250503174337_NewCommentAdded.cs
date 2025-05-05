using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CommentManagement.Infrastructure.EFCore.Migrations
{
    public partial class NewCommentAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments" ,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint" , nullable: false)
                        .Annotation("SqlServer:Identity" , "1, 1") ,
                    Name = table.Column<string>(type: "nvarchar(500)" , maxLength: 500 , nullable: true) ,
                    Email = table.Column<string>(type: "nvarchar(500)" , maxLength: 500 , nullable: true) ,
                    Message = table.Column<string>(type: "nvarchar(2000)" , maxLength: 2000 , nullable: true) ,
                    Website = table.Column<string>(type: "nvarchar(500)" , maxLength: 500 , nullable: true) ,
                    OwnerRecordId = table.Column<long>(type: "bigint" , nullable: false) ,
                    Type = table.Column<int>(type: "int" , nullable: false) ,
                    ParentId = table.Column<long>(type: "bigint" , nullable: false) ,
                    UpdatedDate = table.Column<DateTime>(type: "datetime2" , nullable: false) ,
                    Status = table.Column<int>(type: "int" , nullable: false) ,
                    CreationDate = table.Column<DateTime>(type: "datetime2" , nullable: false)
                } ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments" , x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentId" ,
                        column: x => x.ParentId ,
                        principalTable: "Comments" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentId" ,
                table: "Comments" ,
                column: "ParentId");
            migrationBuilder.Sql(
                        @"ALTER TABLE [Comments] NOCHECK CONSTRAINT [FK_Comments_Comments_ParentId];" );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");
        }
    }
}
