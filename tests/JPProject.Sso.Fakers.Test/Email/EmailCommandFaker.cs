using Bogus;
using JPProject.Sso.Domain.Commands.Email;
using JPProject.Sso.Domain.Models;

namespace JPProject.Sso.Fakers.Test.Email
{
    public static class EmailCommandFaker
    {
        public static Faker<SaveTemplateCommand> GenerateSaveTemplateCommand(string name = null)
        {
            return new Faker<SaveTemplateCommand>().CustomInstantiator(f => new SaveTemplateCommand(
                f.Lorem.Text(),
                f.Lorem.Paragraphs(),
                name ?? f.Internet.DomainName(),
                f.Person.UserName
            ));
        }

        public static Faker<UpdateTemplateCommand> GenerateUpdateTemplateCommand()
        {
            return new Faker<UpdateTemplateCommand>().CustomInstantiator(f => new UpdateTemplateCommand(
                f.Internet.DomainName(),
                f.Lorem.Text(),
                f.Lorem.Paragraphs(),
                f.Internet.DomainName(),
                f.Person.UserName
            ));
        }
        public static Faker<SaveEmailCommand> GenerateSaveEmailCommand(EmailType? emailType = null)
        {
            return new Faker<SaveEmailCommand>().CustomInstantiator(f => new SaveEmailCommand(
                f.Lorem.Paragraphs(),
                new Sender(f.Person.Email, f.Lorem.Text()),
                f.Lorem.Paragraphs(),
                emailType ?? f.PickRandom<EmailType>(),
                f.Person.Email,
                f.Person.UserName
            ));
        }

        public static Faker<RemoveTemplateCommand> GenerateRemoveTemplateCommand()
        {
            return new Faker<RemoveTemplateCommand>().CustomInstantiator(f => new RemoveTemplateCommand(f.Internet.DomainName()));
        }
    }
}