using AutoMapper;
using CocukSubeProject.Entities;
using CocukSubeProject.Models;

namespace CocukSubeProject
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<User, EditUserModel>().ReverseMap();
        }
    }
}
