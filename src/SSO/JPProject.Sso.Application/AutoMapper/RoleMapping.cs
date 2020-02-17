using AutoMapper;

namespace JPProject.Sso.Application.AutoMapper
{
    public static class RoleMapping
    {
        internal static IMapper Mapper { get; }
        static RoleMapping()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<RoleMapperProfile>()).CreateMapper();
        }
    }
}