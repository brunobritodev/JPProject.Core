using AutoMapper;

namespace JPProject.Admin.Application.AutoMapper
{
    public class AdminApiResourceMapper
    {
        internal static IMapper Mapper { get; }
        static AdminApiResourceMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<AdminApiResourceMapperProfile>()).CreateMapper();
        }
    }
}