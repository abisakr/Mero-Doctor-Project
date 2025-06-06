using AutoMapper;
using Mero_Doctor_Project.DTOs.Admin;
using Mero_Doctor_Project.DTOs.Specilization;
using Mero_Doctor_Project.Models;

namespace Mero_Doctor_Project.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Specialization, SpecializationDto>().ReverseMap();
           
        }
    }

}
