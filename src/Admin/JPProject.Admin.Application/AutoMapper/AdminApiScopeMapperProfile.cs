using AutoMapper;
using JPProject.Admin.Application.ViewModels.ApiScopeViewModels;
using JPProject.Admin.Domain.Commands.ApiScope;

namespace JPProject.Admin.Application.AutoMapper
{
    public class AdminApiScopeMapperProfile : Profile
    {
        public AdminApiScopeMapperProfile()
        {
           
            CreateMap<RemoveApiScopeViewModel, RemoveApiScopeCommand>().ConstructUsing(c => new RemoveApiScopeCommand(c.Name));
            CreateMap<SaveApiScopeViewModel, SaveApiScopeCommand>().ConstructUsing(c => new SaveApiScopeCommand( c.Name, c.Description, c.DisplayName, c.Emphasize, c.ShowInDiscoveryDocument, c.UserClaims));
        }
    }
}