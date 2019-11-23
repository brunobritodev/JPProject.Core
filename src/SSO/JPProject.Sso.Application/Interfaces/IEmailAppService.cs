using JPProject.Sso.Application.ViewModels.EmailViewModels;
using JPProject.Sso.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JPProject.Sso.Application.Interfaces
{
    public interface IEmailAppService : IDisposable
    {
        Task<EmailViewModel> FindByType(EmailType type);
        Task<bool> SaveEmail(EmailViewModel command);
        Task<bool> SaveTemplate(TemplateViewModel command);
        Task<IEnumerable<TemplateViewModel>> ListTemplates();
        Task<bool> UpdateTemplate(TemplateViewModel model);
        Task<TemplateViewModel> GetTemplate(string name);
        Task<bool> RemoveTemplate(string name);
    }
}