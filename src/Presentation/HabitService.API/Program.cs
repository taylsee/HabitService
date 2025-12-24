using HabitService.API.Controllers;
using HabitService.Business.Interfaces.IRepositories;
using HabitService.Business.Interfaces.IServices;
using HabitService.Business.Interfaces.Repositories;
using HabitService.Business.Services;
using HabitService.Data.Data;
using HabitService.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using DotNetEnv;

namespace HabitService.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Env.Load(".env");
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Habit Service API",
                    Version = "v1",
                    Description = "Микросервис для управления привычками"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                            {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddDbContext<HabitDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IHabitCatalogService, HabitCatalogService>();
            builder.Services.AddScoped<IUserHabitService, UserHabitService>();
            builder.Services.AddScoped<IHabitCompletionService, HabitCompletionService>();

            builder.Services.AddScoped<IHabitRepository, HabitRepository>();
            builder.Services.AddScoped<IUserHabitRepository, UserHabitRepository>();
            builder.Services.AddScoped<IHabitCompletionRepository, HabitCompletionRepository>();
            var startup = new Startup(builder.Configuration);
            startup.ConfigureServices(builder.Services);
            builder.Services.AddAutoMapper(typeof(Program));

            var app = builder.Build();

            app.UseCors("AllowAll");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<HabitDbContext>();
                await context.Database.MigrateAsync();
            }

            app.Run();
        }
    }
}