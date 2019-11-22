using Bogus;
using JPProject.Sso.Domain.Commands.Email;
using JPProject.Sso.Domain.Models;

namespace JPProject.Sso.Fakers.Test.Email
{
    public static class EmailCommandFaker
    {
        public static Faker<SaveTemplateCommand> GenerateSaveTemplateCommand()
        {
            return new Faker<SaveTemplateCommand>().CustomInstantiator(f => new SaveTemplateCommand(
                f.Lorem.Text(),
                f.Lorem.Paragraphs(),
                f.Internet.DomainName(),
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
        public static Faker<SaveEmailCommand> GenerateSaveEmailCommand()
        {
            return new Faker<SaveEmailCommand>().CustomInstantiator(f => new SaveEmailCommand(
                f.Lorem.Paragraphs(),
                new Sender(f.Internet.DomainName(), f.Lorem.Text()),
                f.Lorem.Paragraphs(),
                f.PickRandom<EmailType>(),
                f.Person.Email,
                f.Person.UserName
            ));
        }
    }
}