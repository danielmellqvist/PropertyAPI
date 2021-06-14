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
            CreateMap<Comment, CommentsForRealEstateDto>()
                .ForMember(destination => destination.UserName, opt => opt.MapFrom(source => source.User.UserName));
            CreateMap<CommentForCreationDto, Comment>();
            CreateMap<CommentForCreationDto, CommentForReturnDto>();


            CreateMap<RealEstate, RealEstatesDto>();
            CreateMap<ConstructionYear, RealEstatePublicDto>();
            CreateMap<RealEstate, RealEstatePublicDto>()
                .IncludeMembers(source => source.ConstructionYear)
                .ForMember(destination => destination.Address, y => y.MapFrom(source => $"{source.Street}, {source.ZipCode} {source.City}"))
                .ForMember(destination => destination.CreatedOn, y => y.MapFrom(source => source.CreatedUtc.ToLocalTime()))
                .ForMember(destination => destination.ConstructionYear, y => y.MapFrom(source => source.ConstructionYear.Year))
                .ReverseMap();

            CreateMap<RealEstate, RealEstatePrivateDto>()
                .ForMember(destination => destination.Address, y => y.MapFrom(source => $"{source.Street}, {source.ZipCode} {source.City}"))
                .ForMember(destination => destination.ConstructionYear, y => y.MapFrom(source => source.ConstructionYear.Year))
                .ForMember(destination => destination.Contact, y => y.MapFrom(source => source.Contact.Telephone))
                .ForMember(destination => destination.RealEstateType, y => y.MapFrom(source => source.RealEstateType.Type))
                ;
            //Continue here


            CreateMap<RealEstate, RealEstateCreatedDto>();

            // marcus added
            CreateMap<RatingAddNewRatingDto, Rating>()
                .ForMember(destinationMember => destinationMember.RatingValue, x => x.MapFrom(sourceMember => sourceMember.RatingValue))
                .ForMember(destinationMember => destinationMember.AboutUserId, x => x.MapFrom(sourceMember => sourceMember.AboutUserId))
                .ForMember(destinationMember => destinationMember.ByUserId, x => x.MapFrom(sourceMember => sourceMember.ByUserId));
        }
    }
}
