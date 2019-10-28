using AutoMapper;
using AutoMapper.Configuration;

namespace JPProject.Sso.Application.AutoMapper
{
    public class SsoMapperConfig
    {
        public static MapperConfigurationExpression RegisterMappings(params Profile[] customProfiles)
        {
            var cfg = new MapperConfigurationExpression();
            cfg.AddProfile(new DomainToViewModelMappingProfile());
            cfg.AddProfile(new ViewModelToDomainMappingProfile());
            return cfg;
        }
    }
}
