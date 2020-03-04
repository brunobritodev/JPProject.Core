using AutoMapper;

namespace JPProject.Admin.Application.AutoMapper
{
    public class AdminIdentityResourceMapper
    {
        internal static IMapper Mapper { get; }
        static AdminIdentityResourceMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<AdminIdentityResourceMapperProfile>()).CreateMapper();
        }
    }
}