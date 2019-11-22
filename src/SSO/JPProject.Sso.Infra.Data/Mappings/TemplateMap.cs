using JPProject.Sso.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JPProject.Sso.Infra.Data.Mappings
{
    public class TemplateMap : IEntityTypeConfiguration<Template>
    {
        public void Configure(EntityTypeBuilder<Template> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Content).HasMaxLength(int.MaxValue).IsRequired();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(250);
        }
    }
}
