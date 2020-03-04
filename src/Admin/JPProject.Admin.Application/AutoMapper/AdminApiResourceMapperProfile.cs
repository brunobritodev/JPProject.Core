using System.Security.Claims;
using AutoMapper;
using IdentityServer4.Models;
using JPProject.Admin.Application.ViewModels.ApiResouceViewModels;
using JPProject.Admin.Domain.Commands;
using JPProject.Admin.Domain.Commands.ApiResource;
using JPProject.Domain.Core.ViewModels;

namespace JPProject.Admin.Application.AutoMapper
{
    public class AdminApiResourceMapperProfile : Profile
    {
        public AdminApiResourceMapperProfile()
        {
            CreateMap<ApiResource, ApiResourceListViewModel>();
            CreateMap<Claim, ClaimViewModel>().ConstructUsing(a => new ClaimViewModel(a.Type, a.Value));
            /*
              * Api Resource commands
              */
            CreateMap<ApiResource, RegisterApiResourceCommand>().ConstructUsing(c => new RegisterApiResourceCommand(c));
            CreateMap<ApiResource, ApiResourceListViewModel>();

            CreateMap<UpdateApiResourceViewModel, UpdateApiResourceCommand>().ConstructUsing(c => new UpdateApiResourceCommand(c, c.OldApiResourceId));
            CreateMap<RemoveApiResourceViewModel, RemoveApiResourceCommand>().ConstructUsing(c => new RemoveApiResourceCommand(c.Name));
            CreateMap<SaveApiSecretViewModel, SaveApiSecretCommand>().ConstructUsing(c => new SaveApiSecretCommand(c.ResourceName, c.Description, c.Value, c.Type, c.Expiration, (int)c.Hash.GetValueOrDefault(HashType.Sha256)));
            CreateMap<RemoveApiSecretViewModel, RemoveApiSecretCommand>().ConstructUsing(c => new RemoveApiSecretCommand(c.Type, c.Value, c.ResourceName));
            CreateMap<RemoveApiScopeViewModel, RemoveApiScopeCommand>().ConstructUsing(c => new RemoveApiScopeCommand(c.Name, c.ResourceName));
            CreateMap<SaveApiScopeViewModel, SaveApiScopeCommand>().ConstructUsing(c => new SaveApiScopeCommand(c.ResourceName, c.Name, c.Description, c.DisplayName, c.Emphasize, c.ShowInDiscoveryDocument, c.UserClaims));
        }
    }
}