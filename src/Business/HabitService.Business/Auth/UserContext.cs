using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace HabitService.Business.Auth;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal User =>
        _httpContextAccessor.HttpContext?.User 
        ?? throw new ApplicationException("User context is unavailable");

    public Guid UserId => User.GetUserId();

    public bool IsInRole(string role) => User.IsInRole(role);
}