using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Mapping
{
    public class MappingProfile : Profile
    {
        // TODO
        public MappingProfile()
        {
            CreateMap<User, CommentFromUserDto>();

            CreateMap<Comment, CommentFromUserDto>();

            CreateMap<RealEstate, RealEstatesDto>();

            CreateMap<RealEstate, RealEstateDto>()
                .ForMember(x => x.Address, y => y.MapFrom(x => $"{x.Street}, {x.ZipCode} {x.City}"))
                .ForMember(x => x.CreatedOn, option => option.MapFrom(x => x.CreatedUtc.ToLocalTime()))
                ;
        }
    }
}
