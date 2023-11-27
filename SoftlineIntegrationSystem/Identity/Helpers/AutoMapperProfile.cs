using AutoMapper;
using SoftlineIntegrationSystem.Identity.Entities;
using SoftlineIntegrationSystem.Identity.Models;

namespace SoftlineIntegrationSystem.Identity.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserModel>();
        CreateMap<RegisterModel, User>();
        CreateMap<UpdateModel, User>();
    }
}
