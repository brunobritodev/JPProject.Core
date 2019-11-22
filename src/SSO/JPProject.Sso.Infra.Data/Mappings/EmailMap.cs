using JPProject.Sso.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JPProject.Sso.Infra.Data.Mappings
{
    public class EmailMap : IEntityTypeConfiguration<Email>
    {
        public void Configure(EntityTypeBuilder<Email> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Subject).IsRequired().HasMaxLength(250);

            builder.OwnsOne(o => o.Sender, c =>
            {
                c.Property(p => p.Address).IsRequired().HasMaxLength(250);
                c.Property(p => p.Name).IsRequired().HasMaxLength(250);
            });
            builder.OwnsOne(typeof(BlindCarbonCopy), "Bcc", o =>
             {
                 o.Ignore("Recipients");
                 o.Property("_recipientsCollection");
             });

        }

    }
}