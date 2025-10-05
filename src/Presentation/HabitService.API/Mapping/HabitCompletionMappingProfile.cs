using AutoMapper;
using HabitService.API.DTOs;
using HabitService.Business.Interfaces.IServices;
using HabitService.Business.Models;

namespace HabitService.API.Mapping
{
    public class HabitCompletionMappingProfile : Profile
    {
        public HabitCompletionMappingProfile()
        {
            CreateMap<HabitCompletion, HabitCompletionResponse>();

            CreateMap<CompleteHabitRequest, HabitCompletion>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserHabitId, opt => opt.Ignore())
                .ForMember(dest => dest.CompletedAt, opt => opt.Ignore());

            CreateMap<HabitProgress, HabitProgressResponse>();
        }
    }
}
