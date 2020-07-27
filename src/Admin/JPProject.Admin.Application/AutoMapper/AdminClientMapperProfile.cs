using AutoMapper;
using IdentityServer4.Models;
using JPProject.Admin.Application.ViewModels.ClientsViewModels;
using JPProject.Admin.Domain.Commands;
using JPProject.Admin.Domain.Commands.Clients;
using JPProject.Domain.Core.ViewModels;
using System.Collections.Generic;

namespace JPProject.Admin.Application.AutoMapper
{
    public class AdminClientMapperProfile : Profile
    {
        public AdminClientMapperProfile()
        {
            CreateMap<ClientClaim, ClientListViewModel>(MemberList.Destination);
            CreateMap<KeyValuePair<string, string>, ClientPropertyViewModel>();
            CreateMap<ClientClaim, ClaimViewModel>().ConstructUsing(a => new ClaimViewModel(a.Type, a.Value));

            /*
            * Client commands
            */
            CreateMap<ClientViewModel, UpdateClientCommand>().ConstructUsing(c => new UpdateClientCommand(c, c.OldClientId));
            CreateMap<RemoveClientSecretViewModel, RemoveClientSecretCommand>().ConstructUsing(c => new RemoveClientSecretCommand(c.Type, c.Value, c.ClientId));
            CreateMap<RemovePropertyViewModel, RemovePropertyCommand>().ConstructUsing(c => new RemovePropertyCommand(c.Key, c.Value, c.ClientId));
            CreateMap<SaveClientSecretViewModel, SaveClientSecretCommand>().ConstructUsing(c => new SaveClientSecretCommand(c.ClientId, c.Description, c.Value, c.Type, c.Expiration, (int)c.Hash.GetValueOrDefault(HashType.Sha256)));
            CreateMap<SaveClientPropertyViewModel, SaveClientPropertyCommand>().ConstructUsing(c => new SaveClientPropertyCommand(c.ClientId, c.Key, c.Value));
            CreateMap<SaveClientClaimViewModel, SaveClientClaimCommand>().ConstructUsing(c => new SaveClientClaimCommand(c.ClientId, c.Type, c.Value));
            CreateMap<RemoveClientClaimViewModel, RemoveClientClaimCommand>().ConstructUsing(c => new RemoveClientClaimCommand(c.Type, c.Value, c.ClientId));
            CreateMap<CopyClientViewModel, CopyClientCommand>().ConstructUsing(c => new CopyClientCommand(c.ClientId));
            CreateMap<SaveClientViewModel, SaveClientCommand>().ConstructUsing(c => new SaveClientCommand(c.ClientId, c.ClientName, c.ClientUri, c.LogoUri, c.Description, c.ClientType, c.LogoutUri));

        }
    }
}