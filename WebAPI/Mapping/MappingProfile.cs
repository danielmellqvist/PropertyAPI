using AutoMapper;
using Entities;
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
        private readonly PropertyContext _context;

        public MappingProfile(PropertyContext context)
        {
            _context = context;
        }


        // TODO
        public MappingProfile()
        {
            CreateMap<User, CommentFromUserDto>();

            CreateMap<Comment, CommentFromUserDto>();

            CreateMap<ConstructionYear, RealEstateDto>();

            CreateMap<RealEstate, RealEstateDto>()
                .IncludeMembers(source => source.ConstructionYear)
                .ForMember(destination => destination.Address, y => y.MapFrom(source => $"{source.Street}, {source.ZipCode} {source.City}"))
                .ForMember(destination => destination.CreatedOn, y => y.MapFrom(source => source.CreatedUtc.ToLocalTime()))
                ;

            CreateMap<CommentForCreationDto, Comment >();

            CreateMap<CommentForCreationDto, CommentForReturnDto>();

            
                .ForMember(destination => destination.ConstructionYear, y => y.MapFrom(source => source.ConstructionYear.Year));

            CreateMap<RealEstate, RealEstateCreatedDto>();

            //CreateMap<RealEstate, RealEstateForCreationDto>()
            //    .IncludeMembers(x => x.Contact, x => x.ConstructionYear)
            //    .ForMember(destination => destination.Contact, opt => opt.MapFrom(source => source.Contact.Telephone))
            //    .ReverseMap();

            //CreateMap<Contact, RealEstateForCreationDto>()
            //    .ForMember(destination => destination.Contact, y => y.MapFrom(source => source.Telephone))
            //    .ReverseMap();
            //CreateMap<ConstructionYear, RealEstateForCreationDto>()
            //    .ForMember(destination => destination.ConstructionYear, y => y.MapFrom(source => source.Year))
            //    .ReverseMap();
        }
        


    }
}
