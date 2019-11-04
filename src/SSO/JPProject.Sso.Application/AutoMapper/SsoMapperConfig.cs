using AutoMapper;
using System.Collections.Generic;

namespace JPProject.Sso.Application.AutoMapper
{
    public class SsoMapperConfig
    {
        public static List<Profile> RegisterMappings()
        {
            var cfg = new List<Profile> { new DomainToViewModelMappingProfile(), new ViewModelToDomainMappingProfile() };
            return cfg;
        }
    }
}
