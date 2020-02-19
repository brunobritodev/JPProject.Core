using AutoMapper;

namespace JPProject.Admin.Application.AutoMapper
{
    public class AdminClientMapper
    {
        internal static IMapper Mapper { get; }
        static AdminClientMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<AdminClientMapperProfile>()).CreateMapper();
        }
    }
}
