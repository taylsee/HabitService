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

namespace HabitService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
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
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
                
            });


            builder.Services.AddDbContext<HabitDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Business Services
            builder.Services.AddScoped<IHabitCatalogService, HabitCatalogService>();
            builder.Services.AddScoped<IUserHabitService, UserHabitService>();
            builder.Services.AddScoped<IHabitCompletionService, HabitCompletionService>();

            // Repositories
            builder.Services.AddScoped<IHabitRepository, HabitRepository>();
            builder.Services.AddScoped<IUserHabitRepository, UserHabitRepository>();
            builder.Services.AddScoped<IHabitCompletionRepository, HabitCompletionRepository>();

            //Mapper
            builder.Services.AddAutoMapper(typeof(Program));

            var app = builder.Build();



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
                context.Database.Migrate();
            }

            app.Run();
        }
    }
}
