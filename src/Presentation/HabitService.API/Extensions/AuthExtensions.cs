using HabitService.Business.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace HabitService.API.Extensions
{
    public static class AuthExtensions
    { 
        internal static IServiceCollection AddAuthenticationInternal(this IServiceCollection services)
        {

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            Environment.GetEnvironmentVariable("JWT_SECRET")!)),
                        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization();
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();

            return services;
        }
    }
}
