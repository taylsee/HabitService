namespace HabitService.Business.Auth;

public interface IUserContext
{
    Guid UserId { get; }
    
    bool IsInRole(string role);
}