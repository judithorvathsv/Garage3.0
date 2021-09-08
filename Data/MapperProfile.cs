using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            /*
            CreateMap<Owner, StudentsIndexViewModel>();
            CreateMap<VehicleType, StudentCreateViewModel>().ReverseMap();

            CreateMap<Vehicle, StudentsDetailsViewModel>()
                .ForMember(
                        dest => dest.Attending,
                        from => from.MapFrom(s => s.Enrollments.Count));

            */
        
        }
    }
}
