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
                .ForMember(destination => destination.Address, y => y.MapFrom(source => $"{source.Street}, {source.ZipCode} {source.City}"))
                .ForMember(destination => destination.CreatedOn, y => y.MapFrom(source => source.CreatedUtc.ToLocalTime()))
                ;

            CreateMap<Comment, CommentForCreationDto>();
        }
    }
}
