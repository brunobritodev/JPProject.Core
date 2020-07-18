using AutoMapper;
using IdentityServer4.Models;
using JPProject.Admin.Application.ViewModels.IdentityResourceViewModels;
using JPProject.Domain.Core.ViewModels;
using System.Security.Claims;

namespace JPProject.Admin.Application.AutoMapper
{
    public class AdminIdentityResourceMapperProfile : Profile
    {
        public AdminIdentityResourceMapperProfile()
        {
            CreateMap<IdentityResource, IdentityResourceListView>(MemberList.Destination);
            CreateMap<Claim, ClaimViewModel>().ConstructUsing(a => new ClaimViewModel(a.Type, a.Value));

        }
    }
}