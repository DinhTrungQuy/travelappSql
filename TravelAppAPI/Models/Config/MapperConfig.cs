using AutoMapper;
using TravelAppAPI.Model;
using TravelAppAPI.Models.Dto;

namespace TravelAppAPI.Models.Config
{
    public class MapperConfig
    {
        public static Mapper Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Place, PlaceDto>().ReverseMap();
                cfg.CreateMap<RegisterDto, User>().ReverseMap();
                cfg.CreateMap<User, UserDto>().ReverseMap();
                cfg.CreateMap<Booking, BookingInputDto>().ReverseMap();
                cfg.CreateMap<Booking, BookingOutputDto>()
                    .ForMember(des => des.UserId, cb => cb.MapFrom(src => src.User.UserId))
                    .ForMember(des => des.PlaceId, cb => cb.MapFrom(src => src.Place.PlaceId))
                    .ReverseMap();
                cfg.CreateMap<Wishlist, WishlistInputDto>().ReverseMap();
                cfg.CreateMap<Wishlist, WishlistOutputDto>()
                    .ForMember(des => des.UserId, cb => cb.MapFrom(src => src.User.UserId))
                    .ForMember(des => des.PlaceId, cb => cb.MapFrom(src => src.Place.PlaceId))
                    .ReverseMap();
                cfg.CreateMap<Rating, RatingInputDto>()
                    .ForMember(des => des.UserId, cb => cb.MapFrom(src => src.User.UserId))
                    .ForMember(des => des.PlaceId, cb => cb.MapFrom(src => src.Place.PlaceId))
                    .ReverseMap();
            });
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
