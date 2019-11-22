using System.Globalization;
using AutoMapper;
using JPProject.Domain.Core.Events;
using JPProject.Sso.Application.EventSourcedNormalizers;
using JPProject.Sso.Application.ViewModels.EmailViewModels;
using JPProject.Sso.Application.ViewModels.RoleViewModels;
using JPProject.Sso.Application.ViewModels.UserViewModels;
using JPProject.Sso.Domain.Models;

namespace JPProject.Sso.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<User, UserViewModel>(MemberList.Destination).ForMember(x => x.LockoutEnd, opt => opt.MapFrom(src => src.LockoutEnd != null ? src.LockoutEnd.Value.DateTime.ToShortDateString() : string.Empty));
            CreateMap<User, UserListViewModel>(MemberList.Destination);

            CreateMap<StoredEvent, EventHistoryData>().ConstructUsing(a => new EventHistoryData(a.Message, a.Id.ToString(), a.Details, a.Timestamp.ToString(CultureInfo.InvariantCulture), a.User, a.MessageType, a.RemoteIpAddress));

            CreateMap<Role, RoleViewModel>(MemberList.Destination);
            CreateMap<UserLogin, UserLoginViewModel>(MemberList.Destination);
            CreateMap<Email, EmailViewModel>(MemberList.Destination);
            CreateMap<Template, TemplateViewModel>(MemberList.Destination);

        }
    }
}
