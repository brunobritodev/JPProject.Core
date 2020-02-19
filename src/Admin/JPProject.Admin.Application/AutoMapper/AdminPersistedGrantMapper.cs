using AutoMapper;

namespace JPProject.Admin.Application.AutoMapper
{
    public class AdminPersistedGrantMapper
    {
        internal static IMapper Mapper { get; }
        static AdminPersistedGrantMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<AdminPersistedGrantMapperProfile>()).CreateMapper();
        }
    }
}