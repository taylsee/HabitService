using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HabitService.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "habits",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    periodindays = table.Column<int>(name: "period_in_days", type: "integer", nullable: false),
                    targetvalue = table.Column<int>(name: "target_value", type: "integer", nullable: false),
                    createdby = table.Column<Guid>(name: "created_by", type: "uuid", nullable: true),
                    createdat = table.Column<DateTime>(name: "created_at", type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_habits", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_habits",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    userid = table.Column<Guid>(name: "user_id", type: "uuid", nullable: false),
                    habitid = table.Column<Guid>(name: "habit_id", type: "uuid", nullable: false),
                    currentvalue = table.Column<int>(name: "current_value", type: "integer", nullable: false),
                    startdate = table.Column<DateTime>(name: "start_date", type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    enddate = table.Column<DateTime>(name: "end_date", type: "timestamp with time zone", nullable: true),
                    isactive = table.Column<bool>(name: "is_active", type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_habits", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_habits_habits_habit_id",
                        column: x => x.habitid,
                        principalTable: "habits",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "habits",
                columns: new[] { "id", "created_at", "created_by", "description", "name", "period_in_days", "target_value" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 10, 4, 16, 18, 35, 586, DateTimeKind.Utc).AddTicks(5675), null, "Поддержание водного баланса организма", "Пить 2 литра воды в день", 1, 2 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2025, 10, 4, 16, 18, 35, 586, DateTimeKind.Utc).AddTicks(5680), null, "Ежедневное чтение для саморазвития", "Читать 30 минут", 1, 30 },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2025, 10, 4, 16, 18, 35, 586, DateTimeKind.Utc).AddTicks(5682), null, "Регулярные физические нагрузки", "Заниматься спортом 3 раза в неделю", 7, 3 },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2025, 10, 4, 16, 18, 35, 586, DateTimeKind.Utc).AddTicks(5684), null, "Ежедневная практика mindfulness", "Медитировать 10 минут", 1, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "ix_habits_created_by",
                table: "habits",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_user_habits_habit_id",
                table: "user_habits",
                column: "habit_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_habits_is_active",
                table: "user_habits",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_user_habits_user_id",
                table: "user_habits",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_habits_user_id_habit_id_unique",
                table: "user_habits",
                columns: new[] { "user_id", "habit_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_habits");

            migrationBuilder.DropTable(
                name: "habits");
        }
    }
}
