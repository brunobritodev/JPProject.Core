using AutoMapper;
using JPProject.Sso.Application.ViewModels.EmailViewModels;
using JPProject.Sso.Domain.Commands.Email;
using JPProject.Sso.Domain.Models;

namespace JPProject.Sso.Application.AutoMapper
{
    public class EmailMapperProfile : Profile
    {
        public EmailMapperProfile()
        {
            /*
             * Email commands
             */
            CreateMap<EmailViewModel, SaveEmailCommand>().ConstructUsing(c => new SaveEmailCommand(c.Content, c.Sender, c.Subject, c.Type, c.Bcc, c.Username));
            CreateMap<TemplateViewModel, SaveTemplateCommand>().ConstructUsing(c => new SaveTemplateCommand(c.Subject, c.Content, c.Name, c.Username));
            CreateMap<TemplateViewModel, UpdateTemplateCommand>().ConstructUsing(c => new UpdateTemplateCommand(c.OldName, c.Subject, c.Content, c.Name, c.Username));


            CreateMap<Email, EmailViewModel>(MemberList.Destination);
            CreateMap<Template, TemplateViewModel>(MemberList.Destination);
        }
    }
}