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

            migrationBuilder.CreateTable(
                name: "HabitCompletions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserHabitId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitCompletions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HabitCompletions_user_habits_UserHabitId",
                        column: x => x.UserHabitId,
                        principalTable: "user_habits",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "habits",
                columns: new[] { "id", "created_at", "created_by", "description", "name", "period_in_days", "target_value" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 12, 24, 20, 4, 29, 577, DateTimeKind.Utc).AddTicks(1091), null, "Поддержание водного баланса организма", "Пить 2 литра воды в день (по 250мл)", 1, 8 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2025, 12, 24, 20, 4, 29, 577, DateTimeKind.Utc).AddTicks(1094), null, "Ежедневная физическая активность для бодрости", "Утренняя зарядка 15 минут", 1, 1 },
                    { new Guid("23456789-2345-2345-2345-234567890123"), new DateTime(2025, 12, 24, 20, 4, 29, 577, DateTimeKind.Utc).AddTicks(1109), null, "Регулярные накопления", "Откладывать 10% от дохода каждый месяц", 30, 1 },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2025, 12, 24, 20, 4, 29, 577, DateTimeKind.Utc).AddTicks(1096), null, "Регулярные интенсивные тренировки", "Спорт 3 раза в неделю", 7, 3 },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2025, 12, 24, 20, 4, 29, 577, DateTimeKind.Utc).AddTicks(1097), null, "Поддержание гигиены полости рта", "Чистить зубы 2 раза в день", 1, 2 },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2025, 12, 24, 20, 4, 29, 577, DateTimeKind.Utc).AddTicks(1099), null, "Ежедневное чтение для саморазвития", "Читать по 30 минут в день", 1, 1 },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2025, 12, 24, 20, 4, 29, 577, DateTimeKind.Utc).AddTicks(1102), null, "Регулярное изучение иностранного языка", "Изучать английский по 20 минут в день", 1, 1 },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2025, 12, 24, 20, 4, 29, 577, DateTimeKind.Utc).AddTicks(1103), null, "Ежедневная практика mindfulness", "Медитировать по 10 минут в день", 1, 1 },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2025, 12, 24, 20, 4, 29, 577, DateTimeKind.Utc).AddTicks(1105), null, "Ежедневные прогулки для снятия стресса", "Прогуливаться на свежем воздухе каждый день", 1, 1 },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2025, 12, 24, 20, 4, 29, 577, DateTimeKind.Utc).AddTicks(1106), null, "Составление плана на следующий день", "Планировать день с вечера", 1, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_HabitCompletions_UserHabitId",
                table: "HabitCompletions",
                column: "UserHabitId");

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
                name: "HabitCompletions");

            migrationBuilder.DropTable(
                name: "user_habits");

            migrationBuilder.DropTable(
                name: "habits");
        }
    }
}
