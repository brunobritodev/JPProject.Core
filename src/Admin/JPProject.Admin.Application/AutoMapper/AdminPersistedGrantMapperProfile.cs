using System.Security.Claims;
using AutoMapper;
using IdentityServer4.Models;
using JPProject.Admin.Application.ViewModels;
using JPProject.Admin.Application.ViewModels.IdentityResourceViewModels;
using JPProject.Admin.Domain.Commands.PersistedGrant;
using JPProject.Domain.Core.ViewModels;

namespace JPProject.Admin.Application.AutoMapper
{
    public class AdminPersistedGrantMapperProfile : Profile
    {
        public AdminPersistedGrantMapperProfile()
        {
            CreateMap<IdentityResource, IdentityResourceListView>(MemberList.Destination);
            CreateMap<Claim, ClaimViewModel>().ConstructUsing(a => new ClaimViewModel(a.Type, a.Value));
            /*
            * Persisted grant
            */
            CreateMap<RemovePersistedGrantViewModel, RemovePersistedGrantCommand>().ConstructUsing(c => new RemovePersistedGrantCommand(c.Key));



        }
    }
}