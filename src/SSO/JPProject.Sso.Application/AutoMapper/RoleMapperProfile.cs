using AutoMapper;
using JPProject.Sso.Application.ViewModels.RoleViewModels;
using JPProject.Sso.Domain.Commands.Role;
using JPProject.Sso.Domain.Models;

namespace JPProject.Sso.Application.AutoMapper
{
    public class RoleMapperProfile : Profile
    {
        public RoleMapperProfile()
        {
            /*
              * Role commands
              */
            CreateMap<RemoveRoleViewModel, RemoveRoleCommand>().ConstructUsing(c => new RemoveRoleCommand(c.Name));
            CreateMap<SaveRoleViewModel, SaveRoleCommand>().ConstructUsing(c => new SaveRoleCommand(c.Name));
            CreateMap<RemoveUserFromRoleViewModel, RemoveUserFromRoleCommand>().ConstructUsing(c => new RemoveUserFromRoleCommand(c.Role, c.Username));

            /*
             * Domain to view model
             */
            CreateMap<Role, RoleViewModel>(MemberList.Destination);
        }
    }
}
