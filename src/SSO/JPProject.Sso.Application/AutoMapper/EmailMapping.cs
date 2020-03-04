using AutoMapper;

namespace JPProject.Sso.Application.AutoMapper
{
    public static class EmailMapping
    {
        internal static IMapper Mapper { get; }
        static EmailMapping()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<EmailMapperProfile>()).CreateMapper();
        }
    }
}