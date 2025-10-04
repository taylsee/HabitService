using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace HabitService.API.DTOs
{
    public class UpdateProgressRequest
    {
        public int NewValue { get; set; }
    }

}
