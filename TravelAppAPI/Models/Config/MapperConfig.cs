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
            });
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
