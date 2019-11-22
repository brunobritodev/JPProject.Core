using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Commands;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.Notifications;
using JPProject.Sso.Domain.Commands.Email;
using JPProject.Sso.Domain.Events.Email;
using JPProject.Sso.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace JPProject.Sso.Domain.CommandHandlers
{
    public class EmailCommandHandler :
        CommandHandler,
        IRequestHandler<SaveTemplateCommand, bool>,
        IRequestHandler<UpdateTemplateCommand, bool>,
        IRequestHandler<SaveEmailCommand, bool>
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IEmailRepository _emailRepository;

        public EmailCommandHandler(
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications,
            ITemplateRepository templateRepository,
            IEmailRepository emailRepository) : base(uow, bus, notifications)
        {
            _templateRepository = templateRepository;
            _emailRepository = emailRepository;
        }


        public async Task<bool> Handle(SaveTemplateCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var template = request.ToModel();
            var templateAlreadyExist = await _templateRepository.Exist(template.Name);
            if (templateAlreadyExist)
            {
                await Bus.RaiseEvent(new DomainNotification("Template", "Template already exist."));
                return false;
            }

            _templateRepository.Add(template);
            if (await Commit())
            {
                await Bus.RaiseEvent(new TemplateAddedEvent(template));
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var template = await _templateRepository.GetByName(request.OldName);
            if (template == null)
            {
                await Bus.RaiseEvent(new DomainNotification("Template", "Template not found."));
                return false;
            }

            template.UpdateTemplate(request.Content, request.Subject, request.Name, request.UserName);
            _templateRepository.Update(template);

            if (await Commit())
            {
                await Bus.RaiseEvent(new TemplateAddedEvent(template));
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(SaveEmailCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var email = await _emailRepository.GetByType(request.Type);
            if (email == null)
            {
                email = request.ToModel();
                _emailRepository.Add(email);
            }
            else
            {
                email.Update(request);
                _emailRepository.Update(email);
            }


            if (await Commit())
            {
                await Bus.RaiseEvent(new EmailSavedEvent(email));
                return true;
            }

            return false;
        }
    }
}
