using System.Threading;
using System.Threading.Tasks;
using JPProject.Sso.Domain.Events.User;
using MediatR;

namespace JPProject.Sso.Domain.EventHandlers
{
    
    public class UserEventHandler :
        INotificationHandler<UserRegisteredEvent>,
        INotificationHandler<EmailConfirmedEvent>
    {
        public Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(EmailConfirmedEvent notification, CancellationToken cancellationToken)
        {
            // Send some e-mail. Alert admin
            return Task.CompletedTask;
        }
    }
}
