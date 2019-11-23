using AutoMapper;
using JPProject.Domain.Core.Bus;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Application.ViewModels.EmailViewModels;
using JPProject.Sso.Domain.Commands.Email;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JPProject.Sso.Application.Services
{
    public class EmailAppService : IEmailAppService
    {
        private IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly IEmailRepository _emailRepository;
        private readonly ITemplateRepository _templateRepository;

        public IMediatorHandler Bus { get; set; }
        public EmailAppService(IMapper mapper,
            IRoleService roleService,
            IMediatorHandler bus,
            IEmailRepository emailRepository,
            ITemplateRepository templateRepository)
        {
            _mapper = mapper;
            _roleService = roleService;
            _emailRepository = emailRepository;
            _templateRepository = templateRepository;
            Bus = bus;
        }
        public async Task<EmailViewModel> FindByType(EmailType type)
        {
            return _mapper.Map<EmailViewModel>(await _emailRepository.GetByType(type));
        }

        public Task<bool> SaveEmail(EmailViewModel model)
        {
            var registerCommand = _mapper.Map<SaveEmailCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public async Task<IEnumerable<TemplateViewModel>> ListTemplates()
        {
            return _mapper.Map<IEnumerable<TemplateViewModel>>(await _templateRepository.All());
        }

        public async Task<TemplateViewModel> GetTemplate(string name)
        {
            return _mapper.Map<TemplateViewModel>(await _templateRepository.GetByName(name));
        }

        public Task<bool> RemoveTemplate(string name)
        {
            return Bus.SendCommand(new RemoveTemplateCommand(name));
        }

        public Task<bool> SaveTemplate(TemplateViewModel model)
        {
            var registerCommand = _mapper.Map<SaveTemplateCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<bool> UpdateTemplate(TemplateViewModel model)
        {
            var registerCommand = _mapper.Map<UpdateTemplateCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
