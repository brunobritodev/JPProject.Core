using JPProject.Domain.Core.Commands;

namespace JPProject.Sso.Domain.Commands.Email
{
    public abstract class TemplateCommand : Command
    {
        public string Subject { get; protected set; }
        public string Content { get; protected set; }
        public string Name { get; protected set; }
        public string OldName { get; protected set; }
        public string UserName { get; protected set; }
        public bool Active { get; protected set; }

    }
}