using AutoMapper;
using SoftlineIntegrationSystem.Models.Dtos;
using SoftlineIntegrationSystem.Models.Entities;

namespace SoftlineIntegrationSystem.Helpers;

public class Profiles : Profile
{
    public Profiles()
    {
        CreateMap<Venue, VenueAddDto>().ReverseMap();
        CreateMap<Venue, VenueUpdateDto>().ReverseMap();
        CreateMap<Venue, VenueDto>().ReverseMap();
        CreateMap<Restaurant, RestaurantDto>().ReverseMap();
    }
}
