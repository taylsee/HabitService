using AutoMapper;
using HabitService.API.DTOs;
using HabitService.Business.Models;

namespace HabitService.API.Mapping
{
    public class HabitMappingProfile : Profile
    {
        public HabitMappingProfile()
        {
            CreateMap<Habit, HabitResponse>();

            CreateMap<CreateHabitRequest, Habit>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());
        }
    }
}
