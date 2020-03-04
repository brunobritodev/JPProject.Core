using Bogus;
using JPProject.Sso.Application.ViewModels.EmailViewModels;
using JPProject.Sso.Domain.Models;

namespace JPProject.Sso.Fakers.Test.Email
{
    public static class EmailFaker
    {
        public static Faker<Template> GenerateTemplate(bool? active = null)
        {
            return new Faker<Template>()
                .RuleFor(t => t.Content, f => f.Lorem.Word())
                .RuleFor(t => t.Name, f => f.Lorem.Word())
                .RuleFor(t => t.Username, f => f.Lorem.Word())
                .RuleFor(t => t.Created, f => f.Date.Past())
                .RuleFor(t => t.Id, f => f.Random.Uuid());
        }

        public static Faker<Domain.Models.Email> GenerateEmail()
        {
            var sender = GenerateSender().Generate();
            return new Faker<Domain.Models.Email>()
                .RuleFor(e => e.Type, f => f.PickRandom<EmailType>())
                .RuleFor(e => e.Subject, f => @"{{picture}}{{name}}{{username}}{{code}}{{url}}{{provider}}{{phoneNumber}}{{email}}")
                .RuleFor(e => e.Bcc, f => f.Internet.Email())
                .RuleFor(e => e.UserName, f => f.Person.UserName)
                .RuleFor(e => e.Id, f => f.Random.Uuid())
                .RuleFor(e => e.Content, f => f.Lorem.Word())
                .RuleFor(e => e.Sender, sender)
                .RuleFor(e => e.Updated, f => f.Date.Past());
        }

        public static Faker<Sender> GenerateSender()
        {
            return new Faker<Sender>().CustomInstantiator(f => new Sender(f.Internet.Email(), f.Company.CompanyName()));
        }


        public static Faker<EmailViewModel> GenerateEmailViewModel(EmailType? type = null)
        {
            var sender = GenerateSender().Generate();
            return new Faker<EmailViewModel>()
                .RuleFor(e => e.Type, f => type ?? f.PickRandom<EmailType>())
                .RuleFor(e => e.Sender, sender)
                .RuleFor(e => e.Subject, f => f.Lorem.Word())
                .RuleFor(e => e.Bcc, f => f.Internet.Email())
                .RuleFor(e => e.Username, f => f.Lorem.Word())
                .RuleFor(e => e.Content, f => f.Lorem.Lines(10));
        }

        public static Faker<TemplateViewModel> GenerateTemplateViewModel()
        {
            return new Faker<TemplateViewModel>()
                .RuleFor(t => t.Subject, f => f.Lorem.Paragraph())
                .RuleFor(t => t.Content, f => f.Lorem.Paragraph())
                .RuleFor(t => t.Name, f => f.Internet.DomainName())
                .RuleFor(t => t.Username, f => f.Person.UserName)
                .RuleFor(t => t.OldName, f => f.Lorem.Paragraph());
        }
    }
}
