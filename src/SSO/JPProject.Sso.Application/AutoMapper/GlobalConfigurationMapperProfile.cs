using AutoMapper;
using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Domain.Commands.GlobalConfiguration;
using JPProject.Sso.Domain.Models;

namespace JPProject.Sso.Application.AutoMapper
{
    public class GlobalConfigurationMapperProfile : Profile
    {
        public GlobalConfigurationMapperProfile()
        {

            /*
             * Global configuration commands
             */
            CreateMap<ConfigurationViewModel, ManageConfigurationCommand>().ConstructUsing(c => new ManageConfigurationCommand(c.Key, c.Value, c.IsSensitive, c.IsPublic));

            CreateMap<GlobalConfigurationSettings, ConfigurationViewModel>(MemberList.Destination).ForMember(m => m.IsSensitive, opt => opt.MapFrom(src => src.Sensitive));

        }
    }
}