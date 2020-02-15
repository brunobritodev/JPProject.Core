using JPProject.EntityFrameworkCore.Interfaces;
using JPProject.Sso.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace JPProject.Sso.Infra.Data.Interfaces
{

    public interface ISsoContext
    {
        DbSet<Template> Templates { get; set; }
        DbSet<Email> Emails { get; set; }
        DbSet<GlobalConfigurationSettings> GlobalConfigurationSettings { get; set; }

    }
}
