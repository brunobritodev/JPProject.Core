using System.Security.Claims;
using AutoMapper;
using IdentityServer4.Models;
using JPProject.Admin.Application.ViewModels.IdentityResourceViewModels;
using JPProject.Admin.Domain.Commands.IdentityResource;
using JPProject.Domain.Core.ViewModels;

namespace JPProject.Admin.Application.AutoMapper
{
    public class AdminIdentityResourceMapperProfile : Profile
    {
        public AdminIdentityResourceMapperProfile()
        {
            CreateMap<IdentityResource, IdentityResourceListView>(MemberList.Destination);
            CreateMap<Claim, ClaimViewModel>().ConstructUsing(a => new ClaimViewModel(a.Type, a.Value));

            /*
             * Identity Resource commands
             */
            CreateMap<IdentityResource, RegisterIdentityResourceCommand>().ConstructUsing(c => new RegisterIdentityResourceCommand(c));
            CreateMap<RemoveIdentityResourceViewModel, RemoveIdentityResourceCommand>().ConstructUsing(c => new RemoveIdentityResourceCommand(c.Name));

        }
    }
}