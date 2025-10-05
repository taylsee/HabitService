using AutoMapper;
using HabitService.API.DTOs;
using HabitService.Business.Models;

namespace HabitService.API.Mapping
{
    public class UserHabitMappingProfile : Profile
    {
        public UserHabitMappingProfile()
        {
            CreateMap<UserHabit, UserHabitResponse>()
                .ForMember(dest => dest.Habit, opt => opt.MapFrom(src => src.Habit))
                .ForMember(dest => dest.CurrentProgress, opt => opt.Ignore())
                .ForMember(dest => dest.IsCompleted, opt => opt.Ignore())
                .ForMember(dest => dest.Remaining, opt => opt.Ignore())
                .ForMember(dest => dest.ProgressPercentage, opt => opt.Ignore());
        }
    }
}
